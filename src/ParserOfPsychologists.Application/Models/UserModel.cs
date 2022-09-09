namespace ParserOfPsychologists.Application.Models;

public record UserModel
{
    public string FullName { get; set; } = string.Empty;
    public string UrlOnSite { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;
    public bool SmsAvailable { get; set; }
    public bool TelegramAvailable { get; set; }
    public bool ViberAvailable { get; set; }
    public bool WhatsAppAvailable { get; set; }

    public string SiteUrl { get; set; } = string.Empty;
    public string TelegramUrl { get; set; } = string.Empty;
    public string VkUrl { get; set; } = string.Empty;
    public string YouTubeUrl { get; set; } = string.Empty;
    public string SkypeNickname { get; set; } = string.Empty;
}