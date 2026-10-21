using AxelLinaresApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxelLinaresApi.Data
{
    public static class DataSeeder 
    {
        public static void Initialize(ProfileDbContext context)
        {
            // Verificación y preparación resiliente del esquema SQLite
            try
            {
                context.Database.EnsureCreated();
                // Verificación de integridad de esquema: comprobar que las columnas coincidan con el modelo
                _ = context.Experiences.Select(e => e.CompanyUrl).FirstOrDefault();
            }
            catch (Exception)
            {
                // Si la estructura cambió (e.g., nuevo campo CompanyUrl), recreamos la base de datos de manera limpia
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            // Si ya existen registros, limpiamos para sincronizar los datos actualizados del seeder
            if (context.UserProfiles.Any())
            {
                context.Projects.RemoveRange(context.Projects);
                context.Certifications.RemoveRange(context.Certifications);
                context.Experiences.RemoveRange(context.Experiences);
                context.UserProfiles.RemoveRange(context.UserProfiles);
                context.SaveChanges();
            }

            var axelProfile = new UserProfile
            {
                Name = "AXEL A. LINARES",
                Title = "Desarrollador Backend Mid-Level | .NET & QA Automation",
                Summary = "Desarrollador Backend Mid-Level y especialista en QA Automation con una obsesión por la estabilidad del sistema y la integridad de los datos. Mi enfoque combina el análisis táctico con el poder de .NET para construir arquitecturas que no solo funcionen, sino que escalen bajo presión. Especializado en: Diseño de APIs REST en C#/.NET, Automatización de pruebas (E2E, Unit Testing), Optimización de bases de datos SQL y Despliegue en cloud (AWS).",
                Email = "axel.a.linares@gmail.com",
                Location = "CDMX, México",
                LinkedInUrl = "https://www.linkedin.com/in/axel-alberto-linares-lorrabaquio-69a6b31b0",
                GitHubUrl = "https://github.com/ElorichV",
                
                HardSkills = new List<string> 
                { 
                    "C#", "Python", "Java", "C++", "JavaScript", "CFML (ColdFusion)", "C",
                    "SQL", "SQL Server", "MongoDB", "SQLite",
                    ".NET 8 / Core", "ASP.NET Core", "Clean Architecture", "Angular", "React",
                    "Playwright (E2E)", "Robot Framework", "NUnit", "Unit Testing", 
                    "AWS", "Azure", "Docker"
                },
                
                SoftSkills = new List<string> 
                { 
                    "Aprendizaje Autónomo (Autodidacta)", 
                    "Resolución de Problemas Complejos",
                    "Comunicación Técnica Efectiva",
                    "Adaptabilidad Tecnológica",
                    "Pensamiento Crítico y Analítico",
                    "Resiliencia bajo Presión"
                },
                
                Experiences = new List<Experience>
                {
                    new Experience 
                    { 
                        Id = 1, 
                        JobTitle = "Full-Stack Software Engineer",
                        Company = "Positronica Labs", 
                        CompanyUrl = "https://positronicalabs.dev/team",
                        Dates = "Enero 2026 - Presente",
                        Responsibilities = new List<string> {
                            "Desarrollo e ingeniería de arquitecturas backend en .NET 8 y microservicios escalables para sistemas agénticos e inteligencia artificial aplicada (sitio oficial: https://positronicalabs.dev/team).",
                            "Liderazgo de la estrategia de aseguramiento de calidad técnica (QA): diseño e implementación de frameworks de pruebas E2E automatizadas con Playwright Sharp y C#.",
                            "Implementación de pipelines de CI/CD con quality gates estrictos para validación de releases, asegurando estabilidad y cero regresiones antes de despliegues productivos.",
                            "Investigación e integración de infraestructura resiliente, sistemas locales cerrados (local-first) y APIs RESTful seguras mitigando riesgos OWASP Top 10."
                        } 
                    },
                    new Experience 
                    { 
                        Id = 2, 
                        JobTitle = "QA Automation Intern",
                        Company = "Sección Amarilla (Agencia Digital ADN)", 
                        CompanyUrl = "https://www.seccionamarilla.com.mx/",
                        Dates = "Marzo 2026 - Presente",
                        Responsibilities = new List<string> {
                            "Desarrollé e implementé estrategias de pruebas automatizadas E2E utilizando .NET 8 y Playwright, asegurando la calidad de plataformas digitales críticas.",
                            "Reduje el tiempo de detección de errores en un 30% mediante la implementación de suites de QA en el ciclo de vida de desarrollo (SDLC).",
                            "Colaboré en la identificación temprana de defectos en aplicaciones empresariales, optimizando flujos de usuario.",
                            "Realicé mantenimiento preventivo del repositorio de demos de Playwright, mejorando la estabilidad del entorno de pruebas."
                        } 
                    },
                    new Experience 
                    { 
                        Id = 3, 
                        JobTitle = "Especialización Técnica y Académica", 
                        Company = "UNITEC & Cisco", 
                        CompanyUrl = "https://www.credly.com/users/axel-alberto-linares-lorrabaquio/badges",
                        Dates = "Febrero 2025 - Febrero 2026", 
                        Responsibilities = new List<string> {
                            "Especialización avanzada en seguridad de red y administración de amenazas bajo los estándares de la Cisco Networking Academy.",
                            "Análisis profundo y respuesta ante incidentes como SOC Analyst, utilizando herramientas de Network Defense."
                        } 
                    },
                    new Experience 
                    { 
                        Id = 4, 
                        JobTitle = "Associate Software Engineer I", 
                        Company = "Solera", 
                        CompanyUrl = "https://www.solera.com",
                        Dates = "Agosto 2022 - Febrero 2025", 
                        Responsibilities = new List<string> {
                            "Desarrollé módulos de backend en C#/.NET para plataformas empresariales, incluyendo APIs REST para gestión de datos.",
                            "Optimicé consultas SQL, reduciendo el tiempo de respuesta en un 40%.",
                            "Participé en revisiones de código y pruebas unitarias, mejorando la calidad del software.",
                            "Integré componentes eficientes en bases de datos relacionales (SQL Server), optimizando el rendimiento."
                        } 
                    }
                },
                
                Certifications = new List<Certification>
                {
                    new Certification { 
                        Id = 1,
                        Name = "PCAP & PCEP", 
                        IssuingOrganization = "Python Institute", 
                        CredentialUrl = "N/A" 
                    },
                    new Certification { 
                        Id = 2,
                        Name = "Cybersecurity Defense Analyst", 
                        IssuingOrganization = "Cisco", 
                        CredentialUrl = "https://www.credly.com/users/axel-alberto-linares-lorrabaquio/badges" 
                    },
                    new Certification { 
                        Id = 3,
                        Name = "Cyber Threat & Network Defense", 
                        IssuingOrganization = "Cisco", 
                        CredentialUrl = "https://www.credly.com/users/axel-alberto-linares-lorrabaquio/badges" 
                    },
                    new Certification { 
                        Id = 4,
                        Name = "CPP", 
                        IssuingOrganization = "C++ Institute", 
                        CredentialUrl = "N/A" 
                    },
                    new Certification { 
                        Id = 5,
                        Name = "Cloud Practitioner (CLF-C02)", 
                        IssuingOrganization = "AWS", 
                        CredentialUrl = "N/A" 
                    }
                },

                Projects = new List<Project>
                {
                    new Project 
                    { 
                        Id = 1,
                        Title = "Sistema de Gestión para Panaderías", 
                        Description = "Ecosistema empresarial de misión crítica diseñado bajo Clean Architecture con .NET 8 y ASP.NET Core. Implementa transacciones de inventario con consistencia ACID en SQL Server, gestión de stock en tiempo real, índices optimizados y patrón Repositorio desacoplado para compras, ventas y control de inventario.",
                        ProjectUrl = "https://github.com/ElorichV/Sistema-de-gestion-para-panaderias"
                    },
                    new Project 
                    { 
                        Id = 2,
                        Title = "QA Automation Suite - Sección Amarilla", 
                        Description = "Suite empresarial de automatización de pruebas End-to-End (E2E) para plataformas de alto tráfico en Sección Amarilla. Desarrollada con Playwright Sharp, .NET 8 y NUnit; implementa Page Object Model (POM), ejecución paralela y suites de regresión continua que reducen la detección de defectos en un 30%.",
                        ProjectUrl = "https://github.com/ElorichV/Playwrigth_Demo"
                    },
                    new Project 
                    { 
                        Id = 3,
                        Title = "TaskMaster-API", 
                        Description = "Showcase empresarial de CFML Moderno desarrollado sobre Lucee REST API. Diseñado bajo principios de Clean Architecture y directivas AppSec (mitigación OWASP Top 10, validación estricta de inputs, parametrización segura de datos contra SQL Injection y respuestas estandarizadas RFC 7807).",
                        ProjectUrl = "https://github.com/ElorichV/TaskMaster-API-Modern-CFML-Showcase-"
                    },
                    new Project 
                    { 
                        Id = 4,
                        Title = "CV Online Fullstack", 
                        Description = "Plataforma web profesional desacoplada con backend en .NET 8 Web API (Clean Architecture, Entity Framework Core, SQLite, Rate Limiting nativo contra spam, validación estricta de inputs y respuestas estándar RFC 7807 ProblemDetails) y cliente frontend SPA interactivo en React con Vite.",
                        ProjectUrl = "https://github.com/ElorichV/cv-react-frontend"
                    },
                    new Project 
                    { 
                        Id = 5,
                        Title = "Códigos de Aprendizaje y Competencia Algorítmica", 
                        Description = "Colección estructurada de soluciones a retos lógicos y algorítmicos complejos de plataformas como Codeforces y HackerRank, implementadas en C++, C# y Python aplicando optimización de complejidad temporal y espacial.",
                        ProjectUrl = "https://github.com/ElorichV/Codigos-de-aprendizaje-y-competencia"
                    }
                }
            };

            context.UserProfiles.Add(axelProfile);
            context.SaveChanges();
        }
    }
}
