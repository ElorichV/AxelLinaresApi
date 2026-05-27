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
            profile.Title = "Software Engineer | .NET Backend & QA Automation";
            profile.Summary = "Mid-Level Backend Developer and QA Automation specialist with an obsession for system stability and data integrity. My approach combines tactical analysis discipline with the power of the .NET stack to build architectures that don't just work—they scale under pressure.";
            
            // Traducir las experiencias laborales mapeadas de tus capturas reales
            foreach (var exp in profile.Experiences)
            {
                if (exp.Company.Contains("Sección Amarilla"))
                {
                    exp.JobTitle = "QA Automation Intern";
                    exp.Responsibilities = new List<string>
                    {
                        "Development and implementation of E2E automated testing strategies using .NET 8 and Playwright to ensure the quality of critical digital platforms.",
                        "Direct collaboration across the Software Development Life Cycle (SDLC) for early defect identification and user workflow optimization.",
                        "Technical analysis and preventive maintenance of the Playwright_Demo repository."
                    };
                }
                else if (exp.Company.Contains("UNITEC"))
                {
                    exp.JobTitle = "Technical & Academic Specialization";
                    exp.Responsibilities = new List<string>
                    {
                        "Advanced specialization in network security and threat management under Cisco Networking Academy standards.",
                        "Deep analysis and incident response as a SOC Analyst, utilizing Network Defense tools."
                    };
                }
                else if (exp.Company.Contains("Solera"))
                {
                    exp.JobTitle = "Associate Software Engineer I";
                    exp.Responsibilities = new List<string>
                    {
                        "Development of software modules and corrective maintenance for Solera's enterprise platforms.",
                        "Implementation of unit tests and participation in cross-code reviews.",
                        "Integration of efficient components in relational databases and SQL query optimization."
                    };
                }
            }

            // Traducir Proyectos dinámicos (en caso de que consumas campos de la API)
            foreach (var proj in profile.Projects)
            {
                if (proj.Title.Contains("Panaderías") || proj.Title.Contains("Bakery"))
                {
                    proj.Title = "Bakery Management System";
                    proj.Description = "Full-Stack ecosystem engineered across consecutive academic courses to validate and stress-test a complex SQL relational database model.";
                }
                // Añade condiciones para tus otros proyectos si vienen desde la base de datos
            }
            // Traducir las habilidades blandas (Soft Skills) al inglés sobre la marcha
            profile.SoftSkills = new List<string> 
            { 
                "Autonomous Learning (Self-taught)", 
                "Complex Problem Solving",
                "Effective Technical Communication",
                "Technological Adaptability",
                "Critical & Analytical Thinking",
                "Resilience under Pressure"
            };
        }

        // 3. Retornamos el objeto mutado con el idioma correspondiente
        return Ok(profile);
    }
}