namespace DotLink.Application.Configuration
{
    public class ClientSettings
    {
        public const string SectionName = "ClientSettings";

        public string BaseUrl { get; set; } = string.Empty;
        public string ResetPasswordPath { get; set; } = string.Empty;
    }
}