using AxelLinaresApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AxelLinaresApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        public ActionResult<UserProfile> GetProfileData()
        {
            var userProfile = new UserProfile
            {
                Name = "Axel A. Linares",
                Title = "Backend Developer | C# | .NET | APIs | AWS",
                Summary = "Soy un desarrollador backend con experiencia en C#, .NET Framework/Core y Angular. Me enfoco en construir soluciones robustas y eficientes utilizando arquitectura limpia, APIs RESTful y metodologías ágiles. Disfruto resolviendo problemas complejos con lógica clara y trabajo en equipo colaborativo.",
                Links = new List<Link>
                {
                    new Link { Name = "Email", Url = "mailto:axel.a.linares@gmail.com" },
                    new Link { Name = "GitHub", Url = "https://github.com/ElorichV" },
                    // Agrega tu LinkedIn aquí cuando lo tengas
                },
                Experiences = new List<WorkExperience>
                {
                    new WorkExperience
                    {
                        JobTitle = "Associate Software Engineer I",
                        Company = "Solera",
                        Dates = "Agosto 2022 - Febrero 2025",
                        Responsibilities = new List<string>
                        {
                            "Desarrollo backend de aplicaciones de escritorio usando C# y .NET Framework.",
                            "Implementación full stack de features web utilizando Adobe ColdFusion.",
                            "Estandarización de procesos de despliegue, mejorando tiempos y reduciendo errores.",
                            "Colaboración con equipos técnicos y de UX para la mejora de productos."
                        }
                    }
                },
                Projects = new List<Project>
                {
                     new Project
                    {
                        Name = "Software de Gestión para Panadería",
                        Description = "Una aplicación de escritorio en .NET para gestionar inventario, ventas y reportes, demostrando arquitectura limpia y manejo de base de datos.",
                        RepoUrl = "https://github.com/ElorichV/SoftwarePanaderia", // <- Ejemplo
                        Technologies = new List<string> { "C#", ".NET", "Entity Framework", "SQL Server" }
                    },
                    new Project
                    {
                        Name = "CV Online Profesional (Este Sitio)",
                        Description = "Portafolio dinámico construido con React y una API en ASP.NET Core, desplegado en AWS.",
                        RepoUrl = "https://github.com/ElorichV/CV-Online-React-API", // <- Ejemplo
                        Technologies = new List<string> { "React", "TypeScript", "ASP.NET Core", "AWS" }
                    }
                },
                Skills = new List<Skill>
                {
                    new Skill { Name = "C# / .NET", Level = "Avanzado", Category="Lenguajes y Frameworks" },
                    new Skill { Name = "SQL / LINQ", Level = "Avanzado", Category="Bases de Datos" },
                    new Skill { Name = "RESTful APIs", Level = "Avanzado", Category="Backend" },
                    new Skill { Name = "AWS", Level = "Intermedio", Category="Cloud" },
                    new Skill { Name = "Angular", Level = "Intermedio", Category="Frontend" },
                    new Skill { Name = "Python", Level = "Intermedio", Category="Lenguajes y Frameworks" },
                    new Skill { Name = "Git & Version Control", Level = "Avanzado", Category="Herramientas" }
                },
                EducationHistory = new List<Education>
                {
                    new Education { Degree = "Ing. en Sistemas de Software", Institution = "Unitec", Dates = "2023 - Actual" }
                },
                Certifications = new List<Certification>
                {
                    new Certification { Name = "AWS Certified Cloud Practitioner (CLF-C02)", Status = "En Progreso" },
                    new Certification { Name = "AWS Certified Solutions Architect - Associate", Status = "En Progreso" }
                },
                Hobbies = new List<Hobby>
                {
                    new Hobby { Name = "Dungeons & Dragons y Juegos de Mesa", Description = "Disfruto de los retos que implican estrategia, colaboración y resolución creativa de problemas en equipo." },
                    new Hobby { Name = "Lectura de Ficción y Novelas Ligeras", Description = "Mi puerta a nuevas ideas y perspectivas, manteniendo mi mente curiosa y en constante aprendizaje." },
                }
            };

            return Ok(userProfile);
        }
    }
}