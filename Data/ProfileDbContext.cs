using AxelLinaresApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AxelLinaresApi.Data
{
    public class ProfileDbContext : DbContext
    {
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options)
        {
        }

        // Estas propiedades se convertirán en tablas en la base de datos
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<WorkExperience> Experiences { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Education> EducationHistory { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
    }
}