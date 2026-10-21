using System.Collections.Generic;

namespace AxelLinaresApi.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }

        public List<Experience> Experiences { get; set; } = new List<Experience>();
        public List<Certification> Certifications { get; set; } = new List<Certification>();
        public List<Project> Projects { get; set; } = new List<Project>();
        
        public List<string> HardSkills { get; set; } = new List<string>();
        public List<string> SoftSkills { get; set; } = new List<string>();
    }
}
