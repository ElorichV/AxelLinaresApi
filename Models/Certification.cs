namespace AxelLinaresApi.Models
{
    public class Certification
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IssuingOrganization { get; set; } = string.Empty;
        public string? CredentialUrl { get; set; }
        public int UserProfileId { get; set; }
    }
}
