namespace AxelLinaresApi.Models
{
    public class Certification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IssuingOrganization { get; set; }
        public string? CredentialUrl { get; set; }
        public int UserProfileId { get; set; }
    }
}