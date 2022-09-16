using CsvHelper.Configuration;
using System.Text;
using ParserOfPsychologists.Application.Models;

namespace ParserOfPsychologists.Application.Configuration;

public class UserModelClassMap : ClassMap<UserModel>
{
    public UserModelClassMap()
    {
        Map(u => u.FullName).Name("FULL NAME");
        Map(u => u.UrlOnSite).Name("URL");
        Map(u => u.Specialty).Name("SPECIALTY");
        Map(u => u.City).Name("CITY");

        Map(u => u.Phone).Name("PHONE").Convert(c => TrimExcess(c.Value.Phone, new[] { " ", "-", " " }));
        Map(u => u.SmsAvailable).Name("SMS");
        Map(u => u.TelegramAvailable).Name("TELEGRAM");
        Map(u => u.ViberAvailable).Name("VIBER");
        Map(u => u.WhatsAppAvailable).Name("WHATSAPP");

        Map(u => u.SiteUrl).Name("PERSONAL SITE");
        Map(u => u.TelegramUrl).Name("TELEGRAM").Convert(c => TelegramUrlOf(c.Value.TelegramUrl));
        Map(u => u.VkUrl).Name("VK");
        Map(u => u.YouTubeUrl).Name("YOUTUBE");
        Map(u => u.SkypeNickname).Name("SKYPE NICKNAME").Convert(c => TrimExcess(c.Value.SkypeNickname, new[] { "Skype:", "?chat" }));
    }

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