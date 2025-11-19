
# AxelLinaresApi (Backend)

API REST desarrollada en .NET 8 para la gestión de un portafolio profesional dinámico. Utiliza una arquitectura **Code-First** con Entity Framework Core y una base de datos autocontenida.

## 🛠 Tecnologías y Arquitectura

- **Framework**: .NET 8 (ASP.NET Core Web API)
- **ORM**: Entity Framework Core 9.0
- **Base de Datos**: SQLite (Autocontenida en el despliegue)
- **Documentación**: Swagger/OpenAPI
- **Infraestructura**: AWS Elastic Beanstalk (Windows Server + IIS)

## 📂 Estructura del Proyecto

- `Controllers/`: Endpoints de la API (e.g., `ProfileController`).
- `Data/`: Lógica de datos (`ProfileDbContext`) y sembrado inicial (`DataSeeder`).
- `Models/`: Definición de entidades (`UserProfile`, `Project`, `Skill`, etc.).
- `Migrations/`: Historial de cambios de esquema de base de datos.
- `Properties/`: Configuraciones de lanzamiento (`launchSettings.json`).

## 🚀 Desarrollo Local

1. **Restaurar dependencias:**
   ```bash
   dotnet restore
   ```

2. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```
   O presiona F5 en Visual Studio. La API estará disponible en el puerto configurado (ej. https://localhost:7081) y abrirá Swagger automáticamente.

## ☁️ Despliegue en AWS Elastic Beanstalk

### URL de Producción
El endpoint principal de datos se encuentra en: http://axellinaresapi-env.eba-paimdqea.us-east-2.elasticbeanstalk.com/api/Profile

### Configuración Exitosa del Entorno
Para replicar este despliegue, se deben usar las siguientes configuraciones específicas:

- **Plataforma**: .NET on Windows Server
- **Versión**: Windows Server 2025 con IIS
- **Tipo de Instancia**: t3.micro (Crucial para compatibilidad UEFI y Capa Gratuita)
- **Proxy Inverso**: IIS (Predeterminado)

### Pasos de Publicación (Método Manual)
1. En Visual Studio, clic derecho al proyecto -> Publicar.
2. Seleccionar destino: Carpeta.
3. Generar la publicación.
4. Ir a la carpeta de salida y comprimir el contenido (los archivos) en un .zip.
5. Subir el .zip manualmente en la consola de Elastic Beanstalk.

## 🔧 Bitácora de Problemas y Soluciones (Troubleshooting)
Durante el ciclo de vida de desarrollo y despliegue, se resolvieron los siguientes desafíos técnicos críticos:

### 1. Error de Despliegue: "Instance type not eligible for Free Tier"
**Síntoma**: El entorno fallaba al crearse (CREATE_FAILED) porque intentaba lanzar instancias t3.medium o t3.large.

**Solución**: Se configuró manualmente la sección "Capacidad" para eliminar las instancias por defecto y seleccionar únicamente t3.micro, asegurando el uso de la Capa Gratuita de AWS.

### 2. Error de Compatibilidad: "UEFI boot mode not supported on t2.micro"
**Síntoma**: Se intentó usar t2.micro (la instancia gratuita clásica), pero falló porque Windows Server 2025 requiere arranque UEFI, que t2 no soporta.

**Solución**: Se migró la configuración a t3.micro, que pertenece a la familia Nitro, soporta UEFI y también es elegible para la capa gratuita.

### 3. Error de Salud: Estado "Severe / No Data"
**Síntoma**: El entorno se creaba pero quedaba en rojo. Elastic Beanstalk no recibía respuesta del Health Check.

**Causa**: El Health Check por defecto busca en la raíz /, pero la API no tiene nada ahí; solo responde en /api/Profile.

**Solución**: Se modificó la configuración del Load Balancer (Procesos) para cambiar la "Ruta de comprobación de estado" a: /api/Profile.

### 4. Error en Runtime: "Command hooks failed" (Crash al inicio)
**Síntoma**: La aplicación fallaba inmediatamente al arrancar en el servidor nuevo.

**Causa**: La aplicación intentaba "sembrar" datos, pero la base de datos profile.db no existía físicamente en el servidor nuevo.

**Solución**: Se agregó context.Database.Migrate(); en Program.cs justo antes del Seeder. Esto fuerza a EF Core a crear el archivo de base de datos automáticamente al iniciar la app.

### 5. Error de Red: "Invalid option value: 'null' for Subnets"
**Síntoma**: Fallo al crear el entorno por configuración de VPC incompleta.

**Solución**: En el paso de configuración de redes, se marcaron explícitamente todas las casillas de las subredes disponibles (us-east-2a, 2b, 2c).

### 6. Error de Desarrollo: "Espacio de nombres duplicado"
**Síntoma**: El proyecto dejó de compilar localmente.

**Causa**: Código del controlador se copió accidentalmente dentro de un archivo de Migración generado automáticamente.

**Solución**: Se eliminó la carpeta Migrations y el archivo profile.db local para regenerar una migración limpia (Add-Migration InitialCreate).

## Características Clave Implementadas

- **Inyección de Dependencias**: Configurada en Program.cs para el DbContext.
- **Code-First Migration**: La estructura de la BD se genera desde el código C#.
- **Auto-Seeding Inteligente**: El sistema detecta si la BD está vacía y la puebla con el perfil profesional automáticamente.
- **CORS Global**: Política configurada para permitir peticiones desde el Frontend (S3/CloudFront).