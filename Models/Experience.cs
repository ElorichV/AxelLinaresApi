using System.Collections.Generic;

namespace AxelLinaresApi.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string? CompanyUrl { get; set; }
        public string Dates { get; set; } = string.Empty;
        public List<string> Responsibilities { get; set; } = new List<string>();
        public int UserProfileId { get; set; }
    }
}
