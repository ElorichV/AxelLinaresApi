// Data/DataSeeder.cs
// Data/DataSeeder.cs
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
                Title = "Ingeniero de Software Mid-Level | .NET Backend & QA Automation",
                Summary = "Ingeniero de Software con más de 2.5 años de experiencia en desarrollo Backend (.NET) y automatización de calidad (QA). Mi enfoque combina la disciplina del análisis táctico con la potencia del stack .NET para construir arquitecturas estables e integras. Experto en flujos de trabajo E2E con Playwright y Robot Framework, especializándome actualmente en seguridad de redes y ecosistemas Cloud.",
                Email = "axel.a.linares@gmail.com",
                Phone = "5542284164",
                Location = "CDMX, México",
                LinkedInUrl = "https://www.linkedin.com/in/axel-alberto-linares-lorrabaquio-69a6b31b0",
                GitHubUrl = "https://github.com/ElorichV",
                
                // Habilidades separadas lógicamente para mantener el formato de "etiquetas" en el Frontend
                HardSkills = new List<string> 
                { 
                    // Lenguajes de Programación
                    "C#", "Python", "Java", "C++", "JavaScript", "CFML (ColdFusion)", "C",
                    // Bases de Datos
                    "SQL", "SQL Server", "MongoDB",
                    // Frameworks y Tecnologías Core
                    ".NET 8 / Core", "ASP.NET", "Angular","React",
                    // Herramientas de QA y Automatización
                    "Playwright (E2E)", "Robot Framework", "Unit Testing", 
                    // Nube e Infraestructura
                    "AWS", "Azure"
                },
                
                // Competencias profesionales (Soft Skills) enfocadas a un rol Mid-Level
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
                            "Desarrollo e implementación de estrategias de pruebas automatizadas E2E utilizando .NET 8 y Playwright para asegurar la calidad de plataformas digitales críticas.",
                            "Colaboración directa en el ciclo de vida de desarrollo de software (SDLC) para la identificación temprana de defectos y optimización de flujos de usuario.",
                            "Análisis técnico y mantenimiento preventivo del repositorio de demos Playwright_Demo."
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
                            "Desarrollo de módulos de software y mantenimiento correctivo para plataformas empresariales de Solera.",
                            "Implementación de pruebas unitarias y participación en revisiones de código cruzadas.",
                            "Integración de componentes eficientes en bases de datos relacionales y optimización de consultas SQL."
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