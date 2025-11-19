# AxelLinaresApi

API REST desarrollada en .NET 8 para gestión de perfiles de usuario con Entity Framework Core y SQLite.

## Tecnologías

- .NET 8
- Entity Framework Core 9.0
- SQLite
- Swagger/OpenAPI
- AWS Elastic Beanstalk

## Estructura

- `Controllers/` - Controladores de la API
- `Data/` - Contexto de base de datos y seeder
- `Models/` - Modelos de datos
- `Migrations/` - Migraciones de Entity Framework
- `.ebextensions/` - Configuraciones de Elastic Beanstalk

## Desarrollo Local

1. Restaurar paquetes:
```bash
dotnet restore
```

2. Ejecutar la aplicación:
```bash
dotnet run
```

La API estará disponible en `https://localhost:7xxx` con documentación Swagger.

## Despliegue en AWS Elastic Beanstalk

### URL de Producción
```
http://axellinaresapi-env.eba-paimdqea.us-east-2.elasticbeanstalk.com/
```

### Pasos de Despliegue

#### 1. Preparación del Proyecto
```bash
# Crear build para Linux
dotnet publish -c Release -o ./publish-linux --runtime linux-x64 --self-contained false
```

#### 2. Configuración de Elastic Beanstalk
- **Plataforma**: .NET 8 running on 64bit Amazon Linux 2023
- **Preset**: Instancia única (compatible con capa gratuita)
- **Tipo de instancia**: t3.micro
- **Servidor proxy**: Nginx

#### 3. Configuraciones Importantes
- **Roles de servicio**: aws-elasticbeanstalk-service-role
- **Perfil de instancia**: aws-elasticbeanstalk-ec2-role
- **VPC**: Por defecto
- **Subredes**: Todas las disponibles marcadas
- **Arquitectura**: x86_64

#### 4. Estructura del .zip
**CRÍTICO**: El .zip debe contener los archivos directamente en la raíz:
```
AxelLinaresApi-v3.zip
├── AxelLinaresApi (ejecutable)
├── AxelLinaresApi.dll
├── AxelLinaresApi.runtimeconfig.json
├── appsettings.json
├── appsettings.Production.json
└── (todas las DLLs y dependencias)
```

### Problemas Comunes y Soluciones

#### Error: "t2.micro instance type does not support UEFI"
**Solución**: Usar t3.micro en lugar de t2.micro

#### Error: "Command hooks failed" en Windows Server
**Solución**: Cambiar a plataforma Linux (.NET 8 on Amazon Linux 2023)

#### Error: "no .runtimeconfig.json file"
**Solución**: Asegurar que el .zip contenga archivos en la raíz, no carpetas anidadas

#### Error: "no such file or directory with file /var/app/staging/publish/"
**Solución**: Comprimir solo el contenido de la carpeta publish-linux, no la carpeta misma

### Configuraciones de Producción

#### appsettings.Production.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=/tmp/profile.db"
  },
  "AllowedHosts": "*"
}
```

#### .ebextensions/01_aspnetcore.config
```yaml
option_settings:
  aws:elasticbeanstalk:application:environment:
    ASPNETCORE_ENVIRONMENT: Production
    ASPNETCORE_URLS: http://+:5000
```

## Características

- CORS habilitado para desarrollo frontend
- Seeding automático de datos
- Base de datos SQLite local
- Migraciones automáticas en producción
- Health check endpoint en `/`
- Configuración específica para AWS Linux

## Endpoints

- `GET /` - Health check (retorna "OK")
- `GET /api/Profile` - Obtiene datos del perfil completo
- `GET /swagger` - Documentación de la API (solo desarrollo)

## Monitoreo

- **Estado del entorno**: Consola de Elastic Beanstalk
- **Logs**: Disponibles en la sección "Logs" de EB
- **Métricas**: CloudWatch automático