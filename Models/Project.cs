namespace AxelLinaresApi.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ProjectUrl { get; set; }
        public int UserProfileId { get; set; }
    }
}
