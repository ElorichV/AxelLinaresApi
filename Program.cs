using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AxelLinaresApi.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. REGISTRO DE SERVICIOS (CONTENEDOR DE INYECCIÓN DE DEPENDENCIAS)
// =========================================================================

// Configuración de los Controladores de la API
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI para documentar y probar endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro de ProfileDbContext usando SQLite. 
// Intenta leer "DefaultConnection" desde appsettings.json; si no existe, usa el fallback local.
builder.Services.AddDbContext<ProfileDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=profile.db"));

// Configuración de CORS (Cross-Origin Resource Sharing)
// Vital para que Angular/React (Frontend) puedan consultar la API sin ser bloqueados por el navegador
builder.Services.AddCors(options =>
    {
    options.AddPolicy("AllowPortfolioFrontend", policy =>
        {
        policy.AllowAnyOrigin()     // Permite peticiones desde cualquier origen (AWS CloudFront, Azure Static Apps, etc.)
              .AllowAnyMethod()     // Permite cualquier método HTTP (GET, POST, PUT, DELETE)
              .AllowAnyHeader();    // Permite cualquier cabecera HTTP
    });
});

var app = builder.Build();

// =========================================================================
// 2. CONFIGURACIÓN DEL PIPELINE DE PETICIONES HTTP (MIDDLEWARES)
// =========================================================================

// Habilitar Swagger solo en ambiente de desarrollo para pruebas locales
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Forzar la redirección segura a HTTPS
app.UseHttpsRedirection();

// Aplicar la política de CORS ANTES de la autorización y mapeo de rutas
app.UseCors("AllowPortfolioFrontend");

app.UseAuthorization();

// Mapear los endpoints automáticos de tus controladores (ej: /api/profile)
app.MapControllers();

// =========================================================================
// 3. EJECUCIÓN AUTOMÁTICA DEL SEEDER DE BASE DE DATOS
// =========================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Solicitamos el contexto de base de datos dentro del scope seguro de memoria
        var context = services.GetRequiredService<ProfileDbContext>();
        
        // Invocamos tu inicializador profesional para verificar o poblar el CV ATS
        DataSeeder.Initialize(context);
    }
    catch (Exception ex)
    {
        // En caso de fallo crítico de infraestructura en la nube, capturamos el log en la consola de AWS/Azure
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error crítico al inicializar la base de datos y sembrar los datos del CV.");
    }
}

// Arrancar la aplicación y escuchar peticiones
app.Run();