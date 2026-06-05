using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AxelLinaresApi.Data;

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
    public async Task<IActionResult> GetProfile([FromQuery] string lang = "es")
    {
        // 1. Extraemos tu perfil con todas sus relaciones desde SQLite
        var profile = await _context.UserProfiles
            .Include(u => u.Experiences)
            .Include(u => u.Projects)
            .Include(u => u.Certifications)
            .FirstOrDefaultAsync();

        if (profile == null)
        {
            return NotFound(new { message = "Sistemas caídos, perfil no encontrado." });
        }

        // 2. Si el cliente solicita inglés, ejecutamos la traducción analítica en memoria
        if (lang.ToLower() == "en")
        {
            profile.Title = "Mid-Level Backend Developer | .NET & QA Automation";
            profile.Summary = "Mid-Level Backend Developer and QA Automation specialist with an obsession for system stability and data integrity. My approach combines tactical analysis with the power of .NET to build architectures that not only work but scale under pressure. Specialized in: REST API design in C#/.NET, Automated testing (E2E, Unit Testing), SQL database optimization, and Cloud deployment (AWS).";
            
            // Traducir las experiencias laborales con métricas de impacto
            foreach (var exp in profile.Experiences)
            {
                if (exp.Company.Contains("Sección Amarilla"))
                {
                    exp.JobTitle = "QA Automation Intern";
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
                    exp.Responsibilities = new List<string>
                    {   
                        "Advanced specialization in network security and threat management under Cisco Networking Academy standards.",
                        "In-depth analysis and incident response as a SOC Analyst, utilizing Network Defense tools."
                    };
                }
                else if (exp.Company.Contains("Solera"))
                {
                    exp.JobTitle = "Associate Software Engineer I";
                    exp.Responsibilities = new List<string>
                    {
                        "Developed C#/.NET backend modules for enterprise platforms, including REST APIs for data management.",
                        "Optimized SQL queries, reducing response time by 40%.",
                        "Participated in code reviews and unit testing, improving overall software quality.",
                        "Integrated efficient components into relational databases (SQL Server), optimizing query performance."
                    };
                }
            }

            // Traducir Proyectos dinámicos con métricas
            foreach (var proj in profile.Projects)
            {
                if (proj.Title.Contains("Panaderías") || proj.Title.Contains("Bakery") || proj.Title.Contains("Gestión"))
                {
                    proj.Title = "Bakery Management System";
                    proj.Description = "Full-Stack ecosystem developed for inventory and sales management. Developed the C#/.NET backend including REST APIs and SQL Server databases with optimized indexes. Impact: Reduced order processing time by 30%.";
                }
                else if (proj.Title.Contains("Playwright"))
                {
                    proj.Title = "QA Automation Suite (Playwright)";
                    proj.Description = "E2E automated testing framework designed with Playwright and C# .NET. Impact: Reduced technical debt by 20% through automated testing and increased test coverage.";
                }
            }
            
            // Traducir las habilidades blandas (Soft Skills) al inglés
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

        // 3. Retornamos el objeto mutado con el idioma correspondiente
        return Ok(profile);
    }
}