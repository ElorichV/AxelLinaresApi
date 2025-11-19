namespace AxelLinaresApi.Models
{
    // --- El Contenedor Principal ---
    public class UserProfile
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string Name { get; set; } = "Axel A. Linares";
        public string Title { get; set; } = "Backend Developer | C# | .NET | APIs | AWS";
        public string Summary { get; set; } = "Desarrollador enfocado en construir soluciones robustas y eficientes, transformando problemas complejos en software limpio y mantenible.";
        public List<Link> Links { get; set; } = new();
        public List<WorkExperience> Experiences { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
        public List<Education> EducationHistory { get; set; } = new();
        public List<Certification> Certifications { get; set; } = new();
        public List<Hobby> Hobbies { get; set; } = new();
    }

    // --- Secciones Detalladas ---

    public class Link
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }

    public class WorkExperience
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string JobTitle { get; set; } = "";
        public string Company { get; set; } = "";
        public string Dates { get; set; } = "";
        public List<string> Responsibilities { get; set; } = new();
    }

    public class Project
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string? ImageUrl { get; set; }
        public string? ProjectUrl { get; set; }
        public string? RepoUrl { get; set; }
        public List<string> Technologies { get; set; } = new();
    }

    public class Skill
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string Name { get; set; } = "";
        public string Level { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public class Education
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string Degree { get; set; } = "";
        public string Institution { get; set; } = "";
        public string Dates { get; set; } = "";
    }

    public class Certification
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
    }

    public class Hobby
    {
        public int Id { get; set; } // <--- AÑADIDO
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }
}