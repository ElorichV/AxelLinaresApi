# Progreso de Despliegue - AxelLinaresApi (Backend)

## Información del Proyecto
**Proyecto**: CV Profesional - API Backend  
**Tecnología**: .NET 8 Web API + Entity Framework Core + SQLite  
**Plataforma de Despliegue**: AWS Elastic Beanstalk  
**Estado**: ✅ **DESPLEGADO Y FUNCIONANDO**

## URL de Producción
```
http://axellinaresapi-env.eba-paimdqea.us-east-2.elasticbeanstalk.com/
```

## Endpoints Disponibles
- `GET /` - Health check (retorna "OK")
- `GET /api/Profile` - Obtiene datos completos del perfil profesional

## Cronología del Despliegue

### Primer Intento: Windows Server ❌
**Fecha**: Noviembre 2025  
**Plataforma**: .NET en Windows Server  
**Resultado**: FALLIDO

**Errores encontrados**:
1. **Error UEFI**: `t2.micro instance type does not support UEFI`
2. **Command Hooks Failed**: Script 01deploy.ps1 falló con código -1

### Segundo Intento: Linux ✅
**Fecha**: Noviembre 2025  
**Plataforma**: .NET 8 running on 64bit Amazon Linux 2023  
**Resultado**: EXITOSO

**Configuración final**:
- **Tipo de instancia**: t3.micro
- **Arquitectura**: x86_64  
- **Servidor proxy**: Nginx
- **Preset**: Instancia única (capa gratuita)

## Configuraciones Clave

### Estructura del Proyecto
```
AxelLinaresApi/
├── Controllers/
│   └── ProfileController.cs
├── Data/
│   ├── ProfileDbContext.cs
│   └── DataSeeder.cs
├── Models/
│   └── UserProfile.cs
├── .ebextensions/
│   └── 01_aspnetcore.config
├── appsettings.Production.json
└── Program.cs
```

### .ebextensions/01_aspnetcore.config
```yaml
option_settings:
  aws:elasticbeanstalk:application:environment:
    ASPNETCORE_ENVIRONMENT: Production
    ASPNETCORE_URLS: http://+:5000
```

### Program.cs - Configuraciones de Producción
```csharp
// Health check endpoint para Elastic Beanstalk
app.MapGet("/", () => "OK");

// CORS para permitir acceso desde el frontend
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configurar puerto para Elastic Beanstalk
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
```

### appsettings.Production.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=profile.db"
  }
}
```

## Proceso de Despliegue

### 1. Preparación del Build
```bash
# Publicar para Linux
dotnet publish -c Release -r linux-x64 --self-contained false -o publish-linux

# Crear ZIP con estructura correcta (archivos en raíz)
cd publish-linux
zip -r ../deploy.zip .
```

### 2. Subida a Elastic Beanstalk
1. Crear aplicación en AWS Elastic Beanstalk
2. Seleccionar plataforma: ".NET 8 running on 64bit Amazon Linux 2023"
3. Subir archivo ZIP con archivos en la raíz (NO carpetas anidadas)
4. Configurar variables de entorno si es necesario

### 3. Verificación
- Health check: `GET /` debe retornar "OK"
- API endpoint: `GET /api/Profile` debe retornar datos JSON

## Lecciones Aprendidas

### ✅ Buenas Prácticas
1. **Linux sobre Windows**: Más estable y económico para .NET Core
2. **Estructura del ZIP**: Archivos deben estar en la raíz, no en subcarpetas
3. **Health Check**: Endpoint `/` es crucial para Elastic Beanstalk
4. **CORS**: Configurar correctamente para permitir acceso desde frontend
5. **Variables de entorno**: Usar `PORT` para configuración dinámica

### ❌ Errores Comunes
1. **Carpetas anidadas en ZIP**: Causa errores de "file not found"
2. **Tipo de instancia incorrecto**: t2.micro no soporta algunas AMI
3. **Falta de health check**: Elastic Beanstalk marca la instancia como unhealthy

## Monitoreo y Logs
- **AWS CloudWatch**: Logs automáticos de la aplicación
- **Elastic Beanstalk Health**: Dashboard de salud de la instancia
- **Application Logs**: Accesibles desde la consola de EB

## Costos
- **Elastic Beanstalk**: Gratis (solo se paga por recursos EC2)
- **EC2 t3.micro**: Incluido en capa gratuita (750 horas/mes)
- **Costo estimado**: $0/mes (dentro de capa gratuita)

## Próximos Pasos
- ✅ API funcionando correctamente
- ✅ Integración con frontend React
- ⏳ Configurar dominio personalizado (opcional)
- ⏳ Implementar HTTPS con Load Balancer (opcional)
- ⏳ Configurar CI/CD con GitHub Actions (opcional)

---
*Documento actualizado: Noviembre 2025*  
*Estado: Producción - Funcionando*