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
                // Verificación de integridad de esquema: comprobar que las columnas nuevas existan
                _ = context.Experiences.Select(e => e.CompanyUrl).FirstOrDefault();
                _ = context.Projects.Select(p => p.Category).FirstOrDefault();
            }
            catch (Exception)
            {
                // Si la estructura cambió, recreamos la base de datos de manera limpia
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
                Name = "AXEL ALBERTO LINARES LORRABAQUIO",
                Title = "Ingeniero de Software | Backend (.NET/C#) & QA Automation",
                Summary = "Ingeniero de Software con 3 años de experiencia diseñando servicios Backend robustos (.NET/C#) y construyendo arquitecturas de automatización de pruebas (QA). Especializado en el desarrollo de APIs REST, optimización avanzada de bases de datos SQL y la creación de suites automatizadas End-to-End (E2E) mediante Playwright. Orientado a garantizar la alta disponibilidad, seguridad y escalabilidad en ecosistemas ágiles y despliegues nativos en la nube (AWS).",
                Email = "axel.a.linares@gmail.com",
                Phone = "5542284164",
                Location = "CDMX, México",
                LinkedInUrl = "https://www.linkedin.com/in/axel-alberto-linares-lorrabaquio-69a6b31b0",
                GitHubUrl = "https://github.com/ElorichV",
                
                HardSkills = new List<string> 
                { 
                    "C#", "Python", "SQL", "Java", "C++", "JavaScript", "CFML",
                    ".NET Core", ".NET Framework", "ASP.NET", "Spring Boot", "APIs REST", "Microservicios", "Angular", "Clean Architecture",
                    "Playwright (E2E)", "Browserbase (Cloud Headless Browsers)", "Robot Framework", "xUnit", "NUnit", "Moq",
                    "AWS (S3, CloudFront)", "Render (Cloud PaaS)", "Git", "JetBrains (Rider, DataGrip, PyCharm)", "Miro (System Design)",
                    "Threat Management", "Network Defense", "Análisis SOC",
                    "Orquestación Multi-Agente (n8n.cloud, Firecrawl)", "Backboard.io (Persistent Memory & 17k+ LLMs)",
                    "Herramientas Autónomas (Devin, Lovable)", "Integración de APIs (OpenAI, ElevenLabs)", "Visión Computacional (MediaPipe)"
                },
                
                SoftSkills = new List<string> 
                { 
                    "Liderazgo Técnico",
                    "Aprendizaje Autónomo (Autodidacta)", 
                    "Resolución de Problemas Complejos",
                    "Comunicación Técnica Efectiva",
                    "Adaptabilidad Tecnológica",
                    "Pensamiento Crítico y Analítico",
                    "Shift-Left Testing & Cultura de Calidad",
                    "Resiliencia bajo Presión"
                },
                
                Experiences = new List<Experience>
                {
                    new Experience 
                    { 
                        Id = 1, 
                        JobTitle = "Software Engineer (Co-Founder)",
                        Company = "Positronica Labs", 
                        CompanyUrl = "https://positronicalabs.dev",
                        Dates = "Enero 2026 - Actualidad",
                        Responsibilities = new List<string> {
                            "Liderazgo técnico y desarrollo backend/full-stack en la fundación de Positronica Labs (positronicalabs.dev), definiendo la arquitectura base y la lógica de negocio para el lanzamiento inicial del producto.",
                            "Implementación de Clean Architecture para el ecosistema en .NET Core, gestionando el despliegue de infraestructura escalable en la nube mediante AWS y Render.",
                            "Diseño de arquitecturas de automatización E2E (Playwright / Browserbase) y orquestación de agentes de IA (n8n.cloud, Backboard.io, Firecrawl), optimizando los ciclos de despliegue y garantizando la calidad del software."
                        } 
                    },
                    new Experience 
                    { 
                        Id = 2, 
                        JobTitle = "QA Automation Intern (Servicio Social)",
                        Company = "Sección Amarilla (Agencia Digital ADN)", 
                        CompanyUrl = "https://www.seccionamarilla.com.mx/",
                        Dates = "Marzo 2026 – Julio 2026",
                        Responsibilities = new List<string> {
                            "Diseño, arquitectura e implementación desde cero de suites de pruebas automatizadas E2E y de caja negra para el portal principal \"PINBOX\" utilizando C#, .NET y Playwright.",
                            "Refactorización y estabilización de scripts de prueba, implementando patrones de diseño (Page Object Model) para reducir la fragilidad del código y facilitar su escalabilidad.",
                            "Integración de prácticas de calidad (Shift-Left Testing) dentro de ciclos de desarrollo ágiles (Scrum), asegurando la detección temprana de defectos críticos y regresiones previas al despliegue en producción.",
                            "Gestión de repositorios mediante Git, estableciendo políticas de control de versiones e ignorado de archivos .gitignore para proteger credenciales y mantener entornos limpios."
                        } 
                    },
                    new Experience 
                    { 
                        Id = 3, 
                        JobTitle = "Associate Software Engineer I", 
                        Company = "Solera", 
                        CompanyUrl = "https://www.solera.com",
                        Dates = "Agosto 2022 – Febrero 2025", 
                        Responsibilities = new List<string> {
                            "Desarrollo, mantenimiento y evolución de sistemas empresariales de alta transaccionalidad (Backend y Full-Stack) utilizando C#, .NET Framework, Java y CFML.",
                            "Construcción y soporte continuo de aplicaciones cliente-servidor (aplicaciones de consola y Windows Forms), asegurando la integridad de la lógica de negocio y la experiencia del usuario final.",
                            "Optimización profunda del rendimiento de bases de datos relacionales, refactorizando procedimientos almacenados, consultas complejas y analizando planes de ejecución en SQL Server, reduciendo los tiempos de respuesta del sistema en un 30%.",
                            "Colaboración activa con equipos multidisciplinarios bajo metodologías ágiles (SCRUM) para estandarizar los pipelines de despliegue y optimizar los flujos de integración continua."
                        } 
                    }
                },
                
                Certifications = new List<Certification>
                {
                    new Certification { 
                        Id = 1,
                        Name = "AWS Certified Cloud Practitioner (CLF-C02)", 
                        IssuingOrganization = "AWS", 
                        CredentialUrl = "https://aws.amazon.com/certification/certified-cloud-practitioner/" 
                    },
                    new Certification { 
                        Id = 2,
                        Name = "Cybersecurity Defense Analyst, Cyber Threat & Network Defense", 
                        IssuingOrganization = "Cisco", 
                        CredentialUrl = "https://www.credly.com/users/axel-alberto-linares-lorrabaquio/badges" 
                    },
                    new Certification { 
                        Id = 3,
                        Name = "Certified Associate (PCAP) & Entry-Level (PCEP) Programmer", 
                        IssuingOrganization = "Python Institute", 
                        CredentialUrl = "https://pythoninstitute.org/pcap" 
                    },
                    new Certification { 
                        Id = 4,
                        Name = "Certified Professional Programmer (CPP)", 
                        IssuingOrganization = "C++ Institute", 
                        CredentialUrl = "https://cppinstitute.org/cpp" 
                    }
                },

                Projects = new List<Project>
                {
                    // ================= TAB 1: AI HACKATHONS & AGENTES =================
                    new Project 
                    { 
                        Id = 1,
                        Title = "BenchEngine / GreenGemma (MLH Hackathon)", 
                        Description = "Galardonado con Mención Honorífica (4to lugar) en MLH. Plataforma de benchmarking energético local para modelos abiertos (Gemma 4) con extracción estructurada vía Firecrawl, autenticación OTP y síntesis de voz con ElevenLabs.",
                        ProjectUrl = "https://github.com/ElorichV/hacktoberfest2026",
                        Category = "Hackathons",
                        Badge = "🏆 4to Lugar MLH Hackathon",
                        TechStack = new List<string> { "Ollama (Gemma 4)", "Firecrawl", "ElevenLabs", "React", "MojoAuth OTP" },
                        ArchitectureNotes = new List<string> {
                            "Inferencia Local vs Cloud: Ejecución de Gemma 4 sobre Ollama para medir consumo energético real por token sin latencia de red.",
                            "Ingesta Web Estructurada: Integración de Firecrawl para transformar documentación técnica en contextos limpios de evaluación.",
                            "Accesibilidad & Seguridad: Flujo passwordless con MojoAuth OTP y telemetría auditiva en tiempo real vía ElevenLabs API."
                        }
                    },
                    new Project 
                    { 
                        Id = 2,
                        Title = "Voz Abierta — Traductor Bidireccional LSM 3D", 
                        Description = "Prototipo desarrollado para el AI Hackathon por México × Clara. Traductor en tiempo real entre Lengua de Señas Mexicana (LSM) y español utilizando visión computacional con MediaPipe, avatar 3D en Three.js y Web Speech API.",
                        ProjectUrl = "https://github.com/ElorichV/Voz-abierta",
                        Category = "Hackathons",
                        Badge = "🚀 AI Hackathon × Clara",
                        TechStack = new List<string> { "MediaPipe", "Three.js", "Web Speech API", "React", "TypeScript" },
                        ArchitectureNotes = new List<string> {
                            "Visión Computacional Edge: Extracción de landmarks corporales y de manos a 60 FPS en el navegador con MediaPipe sin enviar video crudo a servidores externos.",
                            "Motor de Animación 3D: Interpolación cinemática en Three.js para traducir texto/voz a señas fluidas en un avatar tridimensional.",
                            "Impacto Cívico: Diseño de baja latencia pensado para inclusión en ventanillas de atención ciudadana y servicios financieros."
                        }
                    },
                    new Project 
                    { 
                        Id = 3,
                        Title = "Voice2Task — Agente iOS Voz a GitHub Issues", 
                        Description = "Aplicación nativa iOS (SwiftUI + SwiftData) que transcribe notas de voz técnicas en tiempo real con SFSpeechRecognizer, extrae tareas estructuradas mediante OpenAI (gpt-4o-mini) y las sincroniza automáticamente con GitHub Issues.",
                        ProjectUrl = "https://github.com/ElorichV/Voice2Task",
                        Category = "Hackathons",
                        Badge = "📱 Native iOS + AI Agents",
                        TechStack = new List<string> { "SwiftUI", "SwiftData", "OpenAI API", "SFSpeechRecognizer", "GitHub REST API" },
                        ArchitectureNotes = new List<string> {
                            "Persistencia Local-First: Almacenamiento reactivo con SwiftData garantizando cero pérdida de notas de voz aún sin conexión.",
                            "Extracción Determinista con LLMs: Prompts estructurados (JSON Schema) sobre gpt-4o-mini para transformar audio libre en tickets con título, etiquetas y criterios de aceptación.",
                            "Sincronización Bidireccional: Integración directa con GitHub Issues API con manejo resiliente de reintentos y rate limits."
                        }
                    },

                    // ================= TAB 2: BACKEND & CLOUD ARCHITECTURE =================
                    new Project 
                    { 
                        Id = 4,
                        Title = "Sistema ERP/POS de Gestión para Panaderías", 
                        Description = "Ecosistema empresarial Full-Stack diseñado bajo Clean Architecture con .NET 8 Web API, Entity Framework Core y SQL Server. Implementa control de inventario multi-ingrediente con transacciones ACID y cliente SPA en React 18.",
                        ProjectUrl = "https://github.com/ElorichV/Sistema-de-gestion-para-panaderias",
                        Category = "Backend",
                        Badge = "🏛️ Clean Architecture & ACID",
                        TechStack = new List<string> { ".NET 8 Web API", "C#", "SQL Server", "EF Core", "Clean Architecture", "React 18" },
                        ArchitectureNotes = new List<string> {
                            "Consistencia Transaccional ACID: Uso de transacciones explícitas en EF Core para descontar insumos por receta y registrar ventas sin condiciones de carrera.",
                            "Desacoplamiento por Capas: Separación estricta entre Dominio, Aplicación, Infraestructura y Presentación facilitando pruebas unitarias.",
                            "Optimización SQL: Índices compuestos en SQL Server que redujeron el tiempo de consulta de reportes e inventario en un 30%."
                        }
                    },
                    new Project 
                    { 
                        Id = 5,
                        Title = "Portafolio Cloud Engine & API Core (.NET 8 + AWS)", 
                        Description = "Plataforma Full-Stack desacoplada desplegada sobre AWS (S3, CloudFront y Elastic Beanstalk). Backend en .NET 8 con Rate Limiting nativo por IP, sanitización AppSec contra inyección CRLF y respuestas estandarizadas RFC 7807.",
                        ProjectUrl = "https://github.com/ElorichV/AxelLinaresApi",
                        Category = "Backend",
                        Badge = "☁️ AWS S3 + CloudFront + .NET 8",
                        TechStack = new List<string> { ".NET 8", "ASP.NET Core", "AWS S3 & CloudFront", "SQLite", "RFC 7807", "React + Vite" },
                        ArchitectureNotes = new List<string> {
                            "Resiliencia Anti-Spam (Rate Limiting): Middleware FixedWindowRateLimiter particionado por IP real (X-Forwarded-For) devolviendo HTTP 429 en formato RFC 7807.",
                            "Consultas de Baja Latencia: Lecturas optimizadas con AsNoTracking() en Entity Framework Core y negociación bilingüe en memoria (?lang=es|en).",
                            "Seguridad AppSec: Sanitización estricta de cabeceras SMTP y validación por DataAnnotations para mitigar vulnerabilidades OWASP."
                        }
                    },
                    new Project 
                    { 
                        Id = 6,
                        Title = "TaskMaster API — Modern CFML Showcase", 
                        Description = "API RESTful de grado productivo construida 100% en cfscript moderno sobre Lucee y CommandBox. Implementa arquitectura por capas, autenticación stateless JWT (HMAC-SHA256), base de datos H2 embebida y contratos RFC 7807.",
                        ProjectUrl = "https://github.com/ElorichV/TaskMaster-API-Modern-CFML-Showcase",
                        Category = "Backend",
                        Badge = "🛡️ AppSec & OWASP Top 10",
                        TechStack = new List<string> { "Modern CFML", "Lucee", "JWT HMAC-SHA256", "RFC 7807", "H2 Database", "CommandBox" },
                        ArchitectureNotes = new List<string> {
                            "Modernización de Ecosistema Legacy: Demostración de cómo llevar CFML a estándares empresariales modernos (100% cfscript, sin etiquetas acopladas a vistas).",
                            "Seguridad Stateless: Implementación de tokens JWT firmados con HMAC-SHA256 y consultas 100% parametrizadas contra SQL Injection.",
                            "Estandarización de Errores: Mapeo global de excepciones hacia estructuras RFC 7807 ProblemDetails."
                        }
                    },

                    // ================= TAB 3: QA AUTOMATION & ALGORITMIA =================
                    new Project 
                    { 
                        Id = 7,
                        Title = "QA AI-Assistant & Enterprise Playwright Suite", 
                        Description = "Framework de automatización E2E en C# / .NET 9, Playwright y NUnit integrado con un asistente analítico en Python / OpenAI API que captura excepciones de ejecución en tiempo real para diagnosticar causas raíz y sugerir parches.",
                        ProjectUrl = "https://github.com/ElorichV/Playwrigth_Demo",
                        Category = "QA",
                        Badge = "🤖 Playwright + OpenAI API",
                        TechStack = new List<string> { "C#", ".NET 9", "Playwright", "Python", "OpenAI API", "NUnit", "Browserbase" },
                        ArchitectureNotes = new List<string> {
                            "Diagnóstico Asistido por IA: Captura automática de stack traces, DOM snapshots y logs de Playwright enviados a modelos de OpenAI para sugerir correcciones de selectores o código.",
                            "Arquitectura Resiliente (Zero-Flakiness): Implementación de Page Object Model (POM), LocatorHandlers asíncronos y fábricas de datos sintéticos con Bogus.",
                            "Observabilidad E2E: Generación automática de archivos HAR de red y trazas de ejecución compatibles con sesiones cloud en Browserbase."
                        }
                    },
                    new Project 
                    { 
                        Id = 8,
                        Title = "Plataforma \"Mi Primera Chamba\" (Equipo QA Tester)", 
                        Description = "Integrante del equipo de aseguramiento de calidad (QA) ejecutando pruebas funcionales, validación de flujos críticos de usuario y reporte temprano de defectos (Shift-Left Testing) para garantizar la estabilidad previo a despliegues productivos.",
                        ProjectUrl = null, // Confidencial: sin enlace externo expuesto
                        Category = "QA",
                        Badge = "🎯 Shift-Left QA Team",
                        TechStack = new List<string> { "Shift-Left Testing", "QA Funcional & E2E", "Validación de Flujos", "Reporte de Defectos", "Regresión" },
                        ArchitectureNotes = new List<string> {
                            "Aseguramiento de Calidad en Equipo (Shift-Left): Participación directa junto al equipo de desarrollo validando criterios de aceptación antes del merge a producción.",
                            "Validación de Flujos Críticos: Diseño y ejecución de matrices de prueba sobre registro, autenticación, perfiles de candidatos y emparejamiento de vacantes.",
                            "Trazabilidad de Defectos: Documentación estructurada de bugs con pasos reproducibles, evidencia de red/consola y verificación de hotfixes sin regresiones."
                        }
                    },
                    new Project 
                    { 
                        Id = 9,
                        Title = "Algoritmia Avanzada y Programación Competitiva (OMI)", 
                        Description = "Repositorio de más de 50 soluciones de alto rendimiento en C++ para la Olimpiada Mexicana de Informática (OMI) y jueces en línea, implementando estructuras de datos avanzadas y optimización de complejidad.",
                        ProjectUrl = "https://github.com/ElorichV/Codigos-de-aprendizaje-y-competencia",
                        Category = "QA",
                        Badge = "⚡ C++ Advanced DSA",
                        TechStack = new List<string> { "C++", "Treaps", "Persistent Segment Trees", "Graph Theory", "Dynamic Programming" },
                        ArchitectureNotes = new List<string> {
                            "Estructuras de Datos Avanzadas: Implementación desde cero de Árboles de Segmentos Persistentes, Treaps implícitos, Fenwick Trees y Disjoint Set Union (DSU).",
                            "Optimización Extrema de Memoria y CPU: Soluciones diseñadas para cumplir límites estrictos de ejecución (< 1 segundo) y memoria acotada.",
                            "Base algorítmica aplicada al diseño de consultas eficientes y optimización de motores backend."
                        }
                    }
                }
            };

            context.UserProfiles.Add(axelProfile);
            context.SaveChanges();
        }
    }
}
