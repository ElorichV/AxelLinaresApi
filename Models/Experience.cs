namespace AxelLinaresApi.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Dates { get; set; }
        public List<string> Responsibilities { get; set; }
        public int UserProfileId { get; set; }
    }
}