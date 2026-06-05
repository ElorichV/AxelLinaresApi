# ⚙️ Axel Linares API (Backend)
*A robust .NET 8 REST API powering a dynamic professional portfolio.*

🌍 **[Read in English](#english-version)** | 🌎 **[Leer en Español](#versión-en-español)**

---

<a id="english-version"></a>
## 🇺🇸 English Version

### 📌 Overview
This repository contains the backend service for a dynamic professional portfolio. Built with **.NET 8** and following a **Code-First** architecture via Entity Framework Core, it provides secure endpoints to serve profile data, handle contact forms via SMTP, and manage application state with an embedded database, all deployed on AWS infrastructure.

### 🛠️ Tech Stack & Architecture
* **Framework**: .NET 8 (ASP.NET Core Web API)
* **ORM**: Entity Framework Core 9.0
* **Database**: SQLite (Self-contained for agile deployment)
* **API Documentation**: Swagger / OpenAPI
* **Infrastructure**: AWS Elastic Beanstalk (Windows Server 2025, UEFI + IIS)

### ✨ Key Features
* **Dependency Injection**: Fully configured for DbContext and SMTP services.
* **Smart Auto-Seeding**: The application detects empty states and automatically runs database migrations (`context.Database.Migrate()`) and populates initial portfolio data on startup.
* **Global CORS Policy**: Configured to securely accept requests from the designated AWS CloudFront CDN frontend.
* **Health Checks**: Custom load balancer routing tailored for Elastic Beanstalk to monitor API uptime.

### 🚀 Local Setup

**1. Clone & Restore:**
```bash
git clone [https://github.com/your-username/AxelLinaresApi.git](https://github.com/your-username/AxelLinaresApi.git)
cd AxelLinaresApi
dotnet restore
```

**2. Run the Application:**
Launch the application using the .NET CLI or your preferred JetBrains IDE.
```bash
dotnet run
```
*The API will start at `https://localhost:7081` (or your configured port) and launch the Swagger UI automatically.*

---

<a id="versión-en-español"></a>
## 🇲🇽 Versión en Español

### 📌 Descripción General
Este repositorio contiene el servicio backend de un portafolio profesional dinámico. Construido con **.NET 8** y siguiendo una arquitectura **Code-First** mediante Entity Framework Core, proporciona endpoints seguros para servir datos del perfil, gestionar formularios de contacto vía SMTP y manejar el estado de la aplicación con una base de datos embebida, todo desplegado en la infraestructura de AWS.

### 🛠️ Stack Tecnológico y Arquitectura
* **Framework**: .NET 8 (ASP.NET Core Web API)
* **ORM**: Entity Framework Core 9.0
* **Base de Datos**: SQLite (Autocontenida para un despliegue ágil)
* **Documentación API**: Swagger / OpenAPI
* **Infraestructura**: AWS Elastic Beanstalk (Windows Server 2025, UEFI + IIS)

### ✨ Características Principales
* **Inyección de Dependencias**: Configuración completa para DbContext y servicios SMTP.
* **Auto-Seeding Inteligente**: La aplicación detecta estados vacíos, ejecuta migraciones automáticas (`context.Database.Migrate()`) y puebla los datos iniciales al arrancar.
* **Política CORS Global**: Configurada para aceptar peticiones de forma segura desde el frontend alojado en la CDN de AWS CloudFront.
* **Health Checks**: Enrutamiento de balanceador de carga personalizado para monitorizar el tiempo de actividad en Elastic Beanstalk.

### 🚀 Configuración Local

**1. Clonar y Restaurar:**
```bash
git clone [https://github.com/tu-usuario/AxelLinaresApi.git](https://github.com/tu-usuario/AxelLinaresApi.git)
cd AxelLinaresApi
dotnet restore
```

**2. Ejecutar la Aplicación:**
Inicia la aplicación usando la CLI de .NET o tu IDE preferido de JetBrains.
```bash
dotnet run
```
*La API se iniciará en `https://localhost:7081` (o tu puerto configurado) y abrirá la interfaz de Swagger automáticamente.*

---
*Developed by Axel A. Linares - Software Engineer | .NET Backend & QA Automation*