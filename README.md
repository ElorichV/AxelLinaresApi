# AxelLinaresApi (Backend)

[🇺🇸 English](README.md) | [🇪🇸 Español](README.es.md)

---

REST API developed in .NET 8 for managing a dynamic professional portfolio. Uses a **Code-First** architecture with Entity Framework Core and a self-contained database.

## 🛠 Technologies & Architecture

- **Framework**: .NET 8 (ASP.NET Core Web API)
- **ORM**: Entity Framework Core 9.0
- **Database**: SQLite (Self-contained in deployment)
- **Documentation**: Swagger/OpenAPI
- **Infrastructure**: AWS Elastic Beanstalk (Windows Server + IIS)

## 📂 Project Structure

```
AxelLinaresApi/
├── Controllers/           # API endpoints
│   └── ProfileController.cs
├── Data/                  # Data access logic
│   ├── ProfileDbContext.cs
│   └── DataSeeder.cs
├── Models/                # Domain entities
│   └── UserProfile.cs
├── Migrations/            # Database schema history (EF Core)
│   ├── InitialCreate.cs
│   └── ProfileDbContextModelSnapshot.cs
├── Properties/            # Project configurations
│   ├── PublishProfiles/   # Publishing profiles
│   └── launchSettings.json
├── .ebextensions/         # AWS Elastic Beanstalk configurations
│   └── 01_aspnetcore.config
├── .github/workflows/     # CI/CD (GitHub Actions)
├── publish-linux/         # Publishing artifacts
├── Program.cs             # Application entry point
├── profile.db             # SQLite database
├── web.config             # IIS configuration
└── AxelLinaresApi.csproj  # .NET project file
```

## 🚀 Local Development

1. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```
   Or press F5 in Visual Studio. The API will be available on the configured port (e.g., https://localhost:7081) and will open Swagger automatically.

## ☁️ AWS Elastic Beanstalk Deployment

### Production URL
The main data endpoint is located at: http://axellinaresapi-env.eba-paimdqea.us-east-2.elasticbeanstalk.com/api/Profile

### Successful Environment Configuration
To replicate this deployment, use the following specific configurations:

- **Platform**: .NET on Windows Server
- **Version**: Windows Server 2025 with IIS
- **Instance Type**: t3.micro (Critical for UEFI compatibility and Free Tier)
- **Reverse Proxy**: IIS (Default)

### Publishing Steps (Manual Method)
1. In Visual Studio, right-click the project -> Publish.
2. Select target: Folder.
3. Generate the publication.
4. Go to the output folder and compress the content (files) into a .zip.
5. Upload the .zip manually in the Elastic Beanstalk console.

## 🔧 Troubleshooting Log
During the development and deployment lifecycle, the following critical technical challenges were resolved:

### 1. Deployment Error: "Instance type not eligible for Free Tier"
**Symptom**: Environment failed to create (CREATE_FAILED) because it tried to launch t3.medium or t3.large instances.

**Solution**: Manually configured the "Capacity" section to remove default instances and select only t3.micro, ensuring Free Tier usage.

### 2. Compatibility Error: "UEFI boot mode not supported on t2.micro"
**Symptom**: Attempted to use t2.micro (classic free instance), but failed because Windows Server 2025 requires UEFI boot, which t2 doesn't support.

**Solution**: Migrated configuration to t3.micro, which belongs to the Nitro family, supports UEFI, and is also Free Tier eligible.

### 3. Health Error: "Severe / No Data" Status
**Symptom**: Environment was created but remained red. Elastic Beanstalk received no response from Health Check.

**Cause**: Default Health Check looks at root /, but the API has nothing there; it only responds at /api/Profile.

**Solution**: Modified Load Balancer configuration (Processes) to change the "Health check path" to: /api/Profile.

### 4. Runtime Error: "Command hooks failed" (Startup crash)
**Symptom**: Application failed immediately when starting on the new server.

**Cause**: Application tried to "seed" data, but the profile.db database didn't physically exist on the new server.

**Solution**: Added context.Database.Migrate(); in Program.cs just before the Seeder. This forces EF Core to automatically create the database file when starting the app.

### 5. Network Error: "Invalid option value: 'null' for Subnets"
**Symptom**: Failed to create environment due to incomplete VPC configuration.

**Solution**: In the network configuration step, explicitly checked all available subnet boxes (us-east-2a, 2b, 2c).

### 6. Development Error: "Duplicate namespace"
**Symptom**: Project stopped compiling locally.

**Cause**: Controller code was accidentally copied inside an automatically generated Migration file.

**Solution**: Deleted the Migrations folder and local profile.db file to regenerate a clean migration (Add-Migration InitialCreate).

## Key Implemented Features

- **Dependency Injection**: Configured in Program.cs for DbContext.
- **Code-First Migration**: Database structure is generated from C# code.
- **Smart Auto-Seeding**: System detects if database is empty and populates it with professional profile automatically.
- **Global CORS**: Policy configured to allow requests from Frontend (S3/CloudFront).