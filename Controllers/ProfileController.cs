using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AxelLinaresApi.Data;
using AxelLinaresApi.Models;
using System.Threading.Tasks;

namespace AxelLinaresApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileDbContext _context;

        // Inyección de dependencias: El controlador recibe el contexto de la base de datos
        public ProfileController(ProfileDbContext context)
        {
            _context = context;
        }

        // GET: api/Profile
        [HttpGet]
        public async Task<ActionResult<UserProfile>> GetProfileData()
        {
            // Usamos .Include() para traer las tablas relacionadas.
            // Esto evita que las listas lleguen vacías (null) al Frontend.
            var profile = await _context.UserProfiles
                .Include(p => p.Experiences)
                .Include(p => p.Certifications)
                .Include(p => p.Projects)
                .FirstOrDefaultAsync();

            // Si por alguna razón la base de datos está vacía, mandamos un error 404 claro
            if (profile == null)
            {
                return NotFound(new { message = "El perfil no se encontró en la base de datos. Verifica que el Seeder se haya ejecutado." });
            }

            // Si todo está bien, mandamos un código 200 (OK) con el JSON de tu CV
            return Ok(profile);
        }
    }
}