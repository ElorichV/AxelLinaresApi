using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AxelLinaresApi.Data;
using AxelLinaresApi.Models;

namespace AxelLinaresApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileDbContext _context;

        public ProfileController(ProfileDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserProfile), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile([FromQuery] string lang = "es")
        {
            // 1. Extraemos el perfil con todas sus relaciones desde SQLite optimizado con AsNoTracking
            var profile = await _context.UserProfiles
                .Include(u => u.Experiences)
                .Include(u => u.Projects)
                .Include(u => u.Certifications)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (profile == null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    title: "Perfil no encontrado",
                    detail: "No se encontró el perfil profesional en el sistema.");
            }

            // 2. Si el cliente solicita inglés, ejecutamos la traducción analítica en memoria sin mutar la BD
            if (string.Equals(lang, "en", StringComparison.OrdinalIgnoreCase))
            {
                profile.Title = "Mid-Level Backend Developer | .NET & QA Automation";
                profile.Summary = "Mid-Level Backend Developer and QA Automation specialist with an obsession for system stability and data integrity. My approach combines tactical analysis with the power of .NET to build architectures that not only work but scale under pressure. Specialized in: REST API design in C#/.NET, Automated testing (E2E, Unit Testing), SQL database optimization, and Cloud deployment (AWS).";

                // Traducir las experiencias laborales con métricas de impacto y enlaces oficiales
                foreach (var exp in profile.Experiences)
                {
                    if (exp.Company.Contains("Positronica"))
                    {
                        exp.JobTitle = "Full-Stack Software Engineer";
                        exp.CompanyUrl = "https://positronicalabs.dev/team";
                        exp.Dates = "January 2026 - Present";
                        exp.Responsibilities = new List<string>
                        {
                            "Development and architectural engineering of .NET 8 backend services and scalable microservices for applied AI and agentic systems (official site: https://positronicalabs.dev/team).",
                            "Leadership of technical QA automation strategy: design and implementation of end-to-end (E2E) test frameworks using Playwright Sharp and C#.",
                            "Implementation of CI/CD quality gates and release verification protocols to guarantee high reliability, data integrity, and zero regressions.",
                            "Integration of resilient infrastructure, local-first architectures, and secure RESTful APIs mitigating OWASP Top 10 vulnerabilities."
                        };
                    }
                    else if (exp.Company.Contains("Sección Amarilla"))
                    {
                        exp.JobTitle = "QA Automation Intern";
                        exp.CompanyUrl = "https://www.seccionamarilla.com.mx/";
                        exp.Dates = "March 2026 - Present";
                        exp.Responsibilities = new List<string>
                        {
                            "Developed and implemented automated E2E testing strategies using .NET 8 and Playwright, ensuring the quality of critical digital platforms.",
                            "Reduced bug detection time by 30% by implementing QA suites in the software development life cycle (SDLC).",
                            "Collaborated in the early identification of defects in enterprise applications, optimizing user flows.",
                            "Performed preventive maintenance of the Playwright demo repository, improving test environment stability."
                        };
                    }
                    else if (exp.Company.Contains("UNITEC"))
                    {
                        exp.JobTitle = "Technical & Academic Specialization";
                        exp.CompanyUrl = "https://www.credly.com/users/axel-alberto-linares-lorrabaquio/badges";
                        exp.Dates = "February 2025 - February 2026";
                        exp.Responsibilities = new List<string>
                        {   
                            "Advanced specialization in network security and threat management under Cisco Networking Academy standards.",
                            "In-depth analysis and incident response as a SOC Analyst, utilizing Network Defense tools."
                        };
                    }
                    else if (exp.Company.Contains("Solera"))
                    {
                        exp.JobTitle = "Associate Software Engineer I";
                        exp.CompanyUrl = "https://www.solera.com";
                        exp.Dates = "August 2022 - February 2025";
                        exp.Responsibilities = new List<string>
                        {
                            "Developed C#/.NET backend modules for enterprise platforms, including REST APIs for data management.",
                            "Optimized SQL queries, reducing response time by 40%.",
                            "Participated in code reviews and unit testing, improving overall software quality.",
                            "Integrated efficient components into relational databases (SQL Server), optimizing query performance."
                        };
                    }
                }

                // Traducir Proyectos dinámicos con métricas técnicas
                foreach (var proj in profile.Projects)
                {
                    if (proj.Title.Contains("Panaderías") || proj.Title.Contains("Bakery") || proj.Title.Contains("Gestión"))
                    {
                        proj.Title = "Bakery Management System";
                        proj.Description = "Mission-critical enterprise system engineered under Clean Architecture with .NET 8 and ASP.NET Core. Features inventory transactions with ACID guarantees in SQL Server, real-time stock control, database index optimization, and decoupled repository patterns for purchases, sales, and ingredient traceability.";
                    }
                    else if (proj.Title.Contains("Sección Amarilla") || proj.Title.Contains("Playwright"))
                    {
                        proj.Title = "QA Automation Suite - Sección Amarilla";
                        proj.Description = "Enterprise End-to-End (E2E) test automation suite for critical high-traffic platforms at Sección Amarilla. Built with Playwright Sharp, .NET 8, and NUnit; implements Page Object Model (POM), parallel execution, comprehensive reporting, and continuous regression suites reducing defect detection time by 30%.";
                    }
                    else if (proj.Title.Contains("TaskMaster"))
                    {
                        proj.Title = "TaskMaster-API";
                        proj.Description = "Modern CFML / Lucee REST API enterprise showcase. Engineered under Clean Architecture and AppSec security directives (OWASP Top 10 mitigation, strict input validation, parameterized queries preventing SQL injection, and RFC 7807 standardized problem responses).";
                    }
                    else if (proj.Title.Contains("CV Online") || proj.Title.Contains("Fullstack"))
                    {
                        proj.Title = "CV Online Fullstack";
                        proj.Description = "Decoupled fullstack web platform featuring a robust .NET 8 Web API backend (Clean Architecture, Entity Framework Core, SQLite database, native anti-spam Rate Limiting, strict input validation, RFC 7807 ProblemDetails) and an interactive React SPA with Vite.";
                    }
                    else if (proj.Title.Contains("Aprendizaje") || proj.Title.Contains("Competencia"))
                    {
                        proj.Title = "Algorithmic Problem Solving & Code Showcase";
                        proj.Description = "Structured collection of solutions to complex algorithmic challenges from Codeforces and HackerRank, implemented in C++, C#, and Python with optimized time and space complexity.";
                    }
                }

                // Traducir habilidades blandas (Soft Skills) al inglés
                profile.SoftSkills = new List<string> 
                { 
                    "Autonomous Learning (Self-taught)", 
                    "Complex Problem Solving",
                    "Effective Technical Communication",
                    "Technological Adaptability",
                    "Critical & Analytical Thinking",
                    "Resilience Under Pressure"
                };
            }

            // 3. Retornamos el objeto con el idioma correspondiente
            return Ok(profile);
        }
    }
}
