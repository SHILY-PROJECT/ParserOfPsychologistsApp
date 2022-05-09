﻿namespace ParserOfPsychologists.Application.Parser;

public class MainParser : IParser
{
    private readonly HttpClient _client;
    private readonly PageHandler _page;

    public MainParser(HttpClient client, PageHandler page)
    {
        _client = client;
        _page = page;
    }

    public async Task<IEnumerable<UserData>> ParseUsersByCityAsync()
    {
        var users = new List<UserData>();

        while (_page.MoveNextOnPage())
            users.AddRange((await ParseUsersFromPage()).Select(uu => ParseInfoAboutUser(uu)));

        return users.ToList();
    }

    private async Task<IEnumerable<Uri>> ParseUsersFromPage()
    {
        var doc = new HtmlDocument();

        var xPathUserUrl = "//div[contains(@id, 'items_list_main')]/descendant::a[contains(@name, 'spec') and @href!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, _page.CurrentPage);
        msg.Headers.AddRange(new Dictionary<string, string>
        {
            { "Connection", "keep-alive" },
            { "Referer", _page.PrevPage.OriginalString }
        },
        true);

        doc.LoadHtml(await _client.HttpRequestAsync(msg));

        var pages = doc.DocumentNode
            .SelectNodes(xPathUserUrl)
            .Select(hn => new Uri($"{_page.CurrentPage.Scheme}://{_page.CurrentPage.Host}{hn.GetAttributeValue("href", default(string))}"));

        return pages;
    }

    private UserData ParseInfoAboutUser(Uri userUrl)
    {
        var doc = new HtmlDocument();

        var xPathFullName = "//h1[contains(@itemprop, 'name')]";
        var xPathSpecialtyAndCity = "//div[contains(@class, 'status')]/descendant::a[@href!='' and text()!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, userUrl);
        msg.Headers.AddRange(new Dictionary<string, string>
        {
            { "Connection", "keep-alive" },
            { "Referer", _page.CurrentPage.OriginalString }
        },
        true);

        doc.LoadHtml(_client.HttpRequestAsync(msg).Result);

        var user = new UserData(doc.DocumentNode.SelectSingleNode(xPathFullName).InnerText);
        user.ExtractSpecialtyAndCity(doc.DocumentNode.SelectSingleNode(xPathSpecialtyAndCity).InnerText);

        return user with { Contacts = ParseUserСontacts(userUrl, Regex.Match(doc.Text, @"(?<='spec_id_new_ppp',').*?(?=')").Value) };
    }

    private UserContactsData? ParseUserСontacts(Uri userLink, string contactsId)
    {
        var doc = new HtmlDocument();

        var xPathPhone = "//div/descendant::span[contains(@style, 'text-decoration') and text()!='']";
        var xPathMessengersUnderPhone = "//div/descendant::span[contains(@style, 'contact') and text()!='']";

        if (string.IsNullOrWhiteSpace(userLink.OriginalString))
            throw new ArgumentException("Parameter cannot be empty or null.", nameof(userLink));
        if (string.IsNullOrWhiteSpace(contactsId))
            throw new ArgumentException("Parameter cannot be empty or null.", nameof(contactsId));

        var msg = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_page.CurrentPage.Scheme}://{_page.CurrentPage.Host}/telefon_backend.php?mod=spec_id_new&id={contactsId}")
        };
        msg.Headers.AddRange(new Dictionary<string, string>
        {
            { "Connection", "keep-alive" },
            { "Referer", userLink.OriginalString },
            { "X-Requested-With", "XMLHttpRequest" }
        },
        true);

        var resp = _client.HttpRequestAsync(msg).Result;
        var contactsFromBackend = JsonSerializer.Deserialize<ContactsBackendModel>(resp) ?? new();

        doc.LoadHtml(contactsFromBackend.HtmlWithKontaktOne);

        var messengersUnderPhone = doc.DocumentNode
            .SelectNodes(xPathMessengersUnderPhone)
            ?.Select(x => x.InnerText) ?? Array.Empty<string>();

        return new UserContactsData
        {
            Phone = doc.DocumentNode.SelectSingleNode(xPathPhone).InnerHtml,
            TelegramAvailable = messengersUnderPhone.Any(m => m.Contains("Telegram", StringComparison.OrdinalIgnoreCase)),
            WhatsAppAvailable = messengersUnderPhone.Any(m => m.Contains("WhatsApp", StringComparison.OrdinalIgnoreCase)),
            ViberAvailable = messengersUnderPhone.Any(m => m.Contains("Viber", StringComparison.OrdinalIgnoreCase)),

            TelegramUrl = ExtractHref(doc, "//a[contains(@href, 'tg')]"),
            VkUrl = ExtractHref(doc, "//a[contains(@href, 'vk.com')]"),
            YouTubeUrl = ExtractHref(doc, "//a[contains(@href, 'youtube.com')]"),
            SkypeNickname = ExtractHref(doc, "//a[contains(@href, 'Skype')]"),
            SiteUrl = ExtractHref(doc, "//a[contains(@href, 'http') and not(.//span)]")
        };
    }

    private static string ExtractHref(HtmlDocument doc, string xPath) =>
        doc.DocumentNode?.SelectSingleNode(xPath)?.GetAttributeValue("href", string.Empty) ?? string.Empty;
}