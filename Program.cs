// --- USINGS ---
// Estos 'usings' son necesarios para el DbContext, el Seeder y EF Core.
using AxelLinaresApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Necesario para el Logger en el seeder

// --- BUILDER SETUP ---
var builder = WebApplication.CreateBuilder(args);

// 1. A�adir el DbContext para la base de datos SQLite
// Lee la cadena de conexi�n desde appsettings.json
builder.Services.AddDbContext<ProfileDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- APP BUILD ---
var app = builder.Build();

// 2. Ejecutar el "Seeder" (sembrado) de la base de datos al iniciar
// Este bloque se ejecuta una vez al arrancar la API
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Pide una instancia de nuestra base de datos
        var context = services.GetRequiredService<ProfileDbContext>();

        // Aplica migraciones pendientes automáticamente
        context.Database.Migrate(); 

        // Llama a nuestro m�todo est�tico para sembrar los datos
        DataSeeder.Seed(context);
    }
    catch (Exception ex)
    {
        // Captura cualquier error durante el sembrado y lo muestra en la consola
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Un error ocurri� durante el sembrado de la base de datos.");
    }
}

// Configure the HTTP request pipeline.
// Habilita Swagger solo en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige peticiones HTTP a HTTPS
app.UseHttpsRedirection();

// 3. A�adir la pol�tica de CORS
// �Importante! Debe ir ANTES de UseAuthorization
// Esto permite que nuestra app de React (en localhost:5173) haga peticiones a esta API.
app.UseCors(builder => builder
   .AllowAnyOrigin()   // Permite cualquier origen (por simplicidad)
   .AllowAnyMethod()   // Permite cualquier m�todo (GET, POST, etc.)
   .AllowAnyHeader());  // Permite cualquier encabezado

app.UseAuthorization();

// Health check endpoint para Elastic Beanstalk
app.MapGet("/", () => "OK");

app.MapControllers();

// Configurar puerto para Elastic Beanstalk
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");