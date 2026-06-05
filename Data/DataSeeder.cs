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
            try
            {
                context.Database.EnsureCreated();
            }
            catch (Exception)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            if (context.UserProfiles.Any()) return;

            var axelProfile = new UserProfile
            {
                Name = "AXEL A. LINARES",
                Title = "Desarrollador Backend Mid-Level | .NET & QA Automation",
                Summary = "Desarrollador Backend Mid-Level y especialista en QA Automation con una obsesión por la estabilidad del sistema y la integridad de los datos. Mi enfoque combina el análisis táctico con el poder de .NET para construir arquitecturas que no solo funcionen, sino que escalen bajo presión. Especializado en: Diseño de APIs REST en C#/.NET, Automatización de pruebas (E2E, Unit Testing), Optimización de bases de datos SQL y Despliegue en cloud (AWS).",
                Email = "axel.a.linares@gmail.com",
                Location = "CDMX, México",
                LinkedInUrl = "https://www.linkedin.com/in/axel-alberto-linares-lorrabaquio-69a6b31b0",
                
                HardSkills = new List<string> 
                { 
                    "C#", "Python", "Java", "C++", "JavaScript", "CFML (ColdFusion)", "C",
                    "SQL", "SQL Server", "MongoDB",
                    ".NET 8 / Core", "ASP.NET", "Angular","React",
                    "Playwright (E2E)", "Robot Framework", "Unit Testing", 
                    "AWS", "Azure"
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
                        JobTitle = "QA Automation Intern",
                        Company = "Sección Amarilla (Agencia Digital ADN)", 
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
                        Id = 2, 
                        JobTitle = "Especialización Técnica y Académica", 
                        Company = "UNITEC & Cisco", 
                        Dates = "Febrero 2025 - Febrero 2026", 
                        Responsibilities = new List<string> {
                            "Especialización avanzada en seguridad de red y administración de amenazas bajo los estándares de la Cisco Networking Academy.",
                            "Análisis profundo y respuesta ante incidentes como SOC Analyst, utilizando herramientas de Network Defense."
                        } 
                    },
                    new Experience 
                    { 
                        Id = 3, 
                        JobTitle = "Associate Software Engineer I", 
                        Company = "Solera", 
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
                        Name = "PCAP & PCEP", 
                        IssuingOrganization = "Python Institute", 
                        CredentialUrl = "N/A" 
                    },
                    new Certification { 
                        Name = "Cybersecurity Defense Analyst", 
                        IssuingOrganization = "Cisco", 
                        CredentialUrl = "https://www.credly.com/users/axel-alberto-linares-lorrabaquio/badges" 
                    },
                    new Certification { 
                        Name = "Cyber Threat & Network Defense", 
                        IssuingOrganization = "Cisco", 
                        CredentialUrl = "https://www.credly.com/users/axel-alberto-linares-lorrabaquio/badges" 
                    },
                    new Certification { 
                        Name = "CPP", 
                        IssuingOrganization = "C++ Institute", 
                        CredentialUrl = "N/A" 
                    },
                    new Certification { 
                        Name = "Cloud Practitioner (CLF-C02)", 
                        IssuingOrganization = "AWS", 
                        CredentialUrl = "N/A" 
                    }
                },

                Projects = new List<Project>
                {
                    new Project 
                    { 
                        Title = "QA Automation Suite (Playwright)", 
                        Description = "Framework de pruebas automatizadas E2E diseñado con Playwright y C# .NET. Impacto: Reduje la deuda técnica en un 20% mediante pruebas automatizadas y aumenté la cobertura de pruebas.",
                        ProjectUrl = "https://github.com/ElorichV/Playwrigth_Demo"
                    },
                    new Project 
                    { 
                        Title = "Sistema de Gestión para Panaderías", 
                        Description = "Ecosistema Full-Stack desarrollado para gestión de inventarios y ventas. Desarrollé el backend en C#/.NET con bases de datos optimizadas. Impacto: Reduje el tiempo de procesamiento de pedidos en un 30%.",
                        ProjectUrl = "https://github.com/ElorichV/Sistema-de-gestion-para-panaderias"
                    },
                    new Project 
                    { 
                        Title = "CV Online - Frontend (React)", 
                        Description = "El frontend responsivo y de alto impacto interactivo de mi portafolio web profesional, desarrollado utilizando React, Vite y preparado para animaciones avanzadas.",
                        ProjectUrl = "https://github.com/ElorichV/cv-react-frontend"
                    },
                    new Project 
                    { 
                        Title = "CV Online - Backend API (.NET 8)", 
                        Description = "El backend y API REST de mi CV dinámico, construido con .NET 8, ASP.NET Core Web API y Entity Framework Core, configurado con políticas de CORS y seeder resiliente.",
                        ProjectUrl = "https://github.com/ElorichV/AxelLinaresApi"
                    },
                    new Project 
                    { 
                        Title = "Códigos de Aprendizaje y Competencia Algorítmica", 
                        Description = "Colección estructurada de soluciones a retos lógicos y algorítmicos complejos de plataformas como Codeforces. Próximamente actualizado para incluir desafíos en HackerRank.",
                        ProjectUrl = "https://github.com/ElorichV/Codigos-de-aprendizaje-y-competencia"
                    }
                }
            };

            context.UserProfiles.Add(axelProfile);
            context.SaveChanges();
        }
    }
}