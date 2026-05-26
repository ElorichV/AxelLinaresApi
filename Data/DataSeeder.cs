// Data/DataSeeder.cs
using AxelLinaresApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxelLinaresApi.Data
{
    public static class DataSeeder // <--- Usamos tu nombre de clase
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
                Title = "Software Engineer | .NET Backend & QA Automation",
                Summary = "Software Engineer with 2.5+ years of experience in Backend development (.NET) and QA Automation. Expert in building secure applications and automating E2E workflows using Playwright and Robot Framework. Currently specializing in Cybersecurity and Cloud architectures.",
                Email = "axel.a.linares@gmail.com",
                Phone = "5542284164",
                Location = "CDMX, Mexico",
                LinkedInUrl = "https://www.linkedin.com/in/axel-alberto-linares-lorrabaquio-69a6b31b0",
                GitHubUrl = "https://github.com/ElorichV",
                
                HardSkills = new List<string> { "C#", "SQL", "Java", "CFML (ColdFusion)", "Python", "JavaScript", "C++", ".NET Core/Framework", "ASP.NET", "Angular", "Playwright (E2E)", "Robot Framework", "Unit Testing", "AWS" },
                SoftSkills = new List<string> { "Threat Management", "Network Defense", "SOC Analyst skills" },
                
                Experiences = new List<Experience>
                {
                    new Experience 
                    { 
                        Id = 1, 
                        JobTitle = "QA Automation Intern", 
                        Company = "Sección Amarilla (Agencia Digital ADN)", 
                        Dates = "March 2026 - Present", 
                        // ¡Rellena esta lista!
                        Responsibilities = new List<string> {
                            "Desarrollo e implementación de estrategias de pruebas automatizadas E2E utilizando .NET 8 y Playwright para asegurar la calidad de plataformas digitales críticas.",
                            "Colaboración directa en el ciclo de vida de desarrollo de software (SDLC) para la identificación temprana de defectos y optimización de flujos de usuario.",
                            "Análisis técnico y mantenimiento preventivo del repositorio de demos Playwright_Demo.",
                        } 
                    },
                    new Experience 
                    { 
                        Id = 2, 
                        JobTitle = "Technical & Academic Specialization", 
                        Company = "UNITEC & Cisco", 
                        Dates = "February 2025 - February 2026", 
                        Responsibilities = new List<string> {
                            "Especialización avanzada en seguridad de red y administración de amenazas bajo los estándares de la Cisco Networking Academy.",
                            "Análisis profundo y respuesta ante incidentes como SOC Analyst, utilizando herramientas de Network Defense.",
                        } 
                    },
                    new Experience 
                    { 
                        Id = 3, 
                        JobTitle = "Associate Software Engineer I", 
                        Company = "Solera", 
                        Dates = "August 2022 - February 2025", 
                        Responsibilities = new List<string> {
                            "Desarrollo de módulos de software y mantenimiento correctivo para plataformas empresariales de Solera.",
                            "Implementación de pruebas unitarias y participación en revisiones de código cruzadas.",
                            "Integración de componentes eficientes en bases de datos relacionales y optimización de consultas SQL."
                        } 
                    }
                },
                
                Certifications = new List<Certification>
                {
                    // A las que no tienen link les ponemos "N/A" para satisfacer la regla NOT NULL de la base de datos
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
                        Title = "Playwright E2E Automation Demo", 
                        Description = "Repositorio enfocado en la automatización integral de pruebas de principio a fin (E2E) y de caja negra, implementando buenas prácticas con C# .NET y Playwright.",
                        ProjectUrl = "https://github.com/ElorichV/Playwrigth_Demo"
                    },
                    new Project 
                    { 
                        Title = "Sistema de Gestión para Panaderías", 
                        Description = "Desarrollo de un sistema de escritorio/web para la administración de inventario, insumos y ventas enfocado en panaderías. (Proyecto en pausa temporal debido al cumplimiento del servicio social).",
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
                        Description = "Colección estructurada de soluciones a retos lógicos y algorítmicos complejos de plataformas como Codeforces. Próximamente actualizado para incluir desafíos en múltiples lenguajes usando HackerRank.",
                        ProjectUrl = "https://github.com/ElorichV/Codigos-de-aprendizaje-y-competencia"
                    }
                }
            };

            context.UserProfiles.Add(axelProfile);
            context.SaveChanges();
        }
    }
}