using System.Collections.Generic;

namespace AxelLinaresApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ProjectUrl { get; set; }
        public string Category { get; set; } = "Backend";
        public string? Badge { get; set; }
        public List<string> TechStack { get; set; } = new List<string>();
        public List<string> ArchitectureNotes { get; set; } = new List<string>();
        public int UserProfileId { get; set; }
    }
}
