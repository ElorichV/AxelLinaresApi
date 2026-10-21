using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;
using AxelLinaresApi.Data;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. REGISTRO DE SERVICIOS (CONTENEDOR DE INYECCIÓN DE DEPENDENCIAS)
// =========================================================================

// Respuestas estandarizadas RFC 7807 (ProblemDetails) para errores globales de la API
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
    };
});

// Configuración de validación estricta en Model Binding devolviendo RFC 7807 ValidationProblemDetails
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Error de validación en la solicitud",
            Status = StatusCodes.Status400BadRequest,
            Detail = "Uno o más campos enviados no cumplen con los requisitos de validación de seguridad.",
            Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}"
        };
        problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

        return new BadRequestObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});

// Registro de Controladores
builder.Services.AddControllers();

// Configuración de Rate Limiting nativo de .NET 8 (FixedWindow) para mitigar spam y abusos
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/problem+json";
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc6585#section-4",
            Status = StatusCodes.Status429TooManyRequests,
            Title = "Demasiadas solicitudes (Rate Limit)",
            Detail = "Has superado el límite de solicitudes permitidas. Por motivos de seguridad y prevención de spam, intenta de nuevo en unos momentos.",
            Instance = context.HttpContext.Request.Path
        };
        problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
        await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: token);
    };

    // Política Fixed Window para EmailController:
    // Permite un máximo de 3 envíos por ventana de 1 minuto por dirección IP de origen
    options.AddPolicy("EmailRateLimitPolicy", httpContext =>
    {
        // Extracción resiliente de la IP del cliente (compatible con proxies reversos como Nginx / AWS CloudFront / ALB)
        var forwardedHeader = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        var clientIp = forwardedHeader?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()
                       ?? httpContext.Connection.RemoteIpAddress?.ToString()
                       ?? "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientIp,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
    });
});

// Configuración de Swagger/OpenAPI para documentar y probar endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro de ProfileDbContext usando SQLite
builder.Services.AddDbContext<ProfileDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=profile.db"));

// Configuración de CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPortfolioFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// =========================================================================
// 2. CONFIGURACIÓN DEL PIPELINE DE PETICIONES HTTP (MIDDLEWARES)
// =========================================================================

// Manejo global de excepciones conforme al estándar RFC 7807 ProblemDetails
app.UseExceptionHandler();
app.UseStatusCodePages();

// Habilitar Swagger solo en ambiente de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Forzar la redirección segura a HTTPS
app.UseHttpsRedirection();

// Enrutamiento de endpoints
app.UseRouting();

// Aplicar la política de CORS
app.UseCors("AllowPortfolioFrontend");

// Aplicar Rate Limiting nativo después del enrutamiento y antes de los controladores
app.UseRateLimiter();

app.UseAuthorization();

// Endpoint de Health Check para AWS Elastic Beanstalk y monitoreo de uptime
app.MapGet("/", () => Results.Ok(new 
{ 
    status = "Healthy", 
    service = "Axel Linares Portfolio API", 
    framework = ".NET 8",
    timestamp = DateTime.UtcNow 
}));

// Mapear los endpoints de los controladores
app.MapControllers();

// =========================================================================
// 3. EJECUCIÓN AUTOMÁTICA DEL SEEDER DE BASE DE DATOS
// =========================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ProfileDbContext>();
        DataSeeder.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error crítico al inicializar la base de datos SQLite y sembrar los datos del CV.");
    }
}

// Arrancar la aplicación y escuchar peticiones
app.Run();
