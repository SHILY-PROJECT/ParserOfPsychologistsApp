namespace ParserOfPsychologists.Application.Models;

public class UserContactsData
{
    private string _phone = string.Empty;
    private string _skype = string.Empty;
    private string _telegramUrl = string.Empty;

    public string Phone { get => _phone; set => _phone = TrimExcess(value, new[] { " ", "-", " " }); }
    public bool TelegramAvailable { get; set; }
    public bool ViberAvailable { get; set; }
    public bool WhatsAppAvailable { get; set; }

    public string SiteUrl { get; set; } = string.Empty;
    public string TelegramUrl { get => _telegramUrl; set => _telegramUrl = TelegramUrlOf(value); }
    public string VkUrl { get; set; } = string.Empty;
    public string YouTubeUrl { get; set; } = string.Empty;
    public string SkypeNickname { get => _skype; set => _skype = TrimExcess(value, new[] { "Skype:", "?chat" }); }

    private static string TrimExcess(string value, string[] excessValues)
    {
        if (!string.IsNullOrWhiteSpace(value) && new StringBuilder(value) is StringBuilder sb)
        {
            Array.ForEach(excessValues, ev => sb.Replace(ev, ""));
            return sb.ToString();
        }
        return string.Empty;
    }

    private static string TelegramUrlOf(string telegramUrl) =>
        telegramUrl.Split('=').LastOrDefault() is string v && !string.IsNullOrWhiteSpace(v) ? $"https://t.me/{v}" : string.Empty;
}