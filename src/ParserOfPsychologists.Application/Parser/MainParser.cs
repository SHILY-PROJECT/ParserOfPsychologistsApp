namespace ParserOfPsychologists.Application.Parser;

public class MainParser : IParser
{
    private readonly IParserSettings _parserSetting;
    private readonly PageNavigator _pageNavigator;
    private readonly HttpClient _client;

    public MainParser(IParserSettings parserSetting, PageNavigator pageNavigator, HttpClient client)
    {
        _parserSetting = parserSetting;
        _pageNavigator = pageNavigator;
        _client = client;
    }

    public async Task<IEnumerable<UserData>> ParseUsersByCityAsync() =>
        await Task.Run(() => ParseUsersByCity());

    public IEnumerable<UserData> ParseUsersByCity()
    {
        var users = new List<UserData>();

        while (_pageNavigator.MoveNextOnPage())
            users.AddRange(this.ParseUsersFromPage().Select(uu => this.ParseInfoAboutUser(uu)));       

        return users.ToList();
    }

    private IEnumerable<Uri> ParseUsersFromPage()
    {
        var doc = new HtmlDocument();

        var xPathUserUrl = "//div[contains(@id, 'items_list_main')]/descendant::a[contains(@name, 'spec') and @href!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, _pageNavigator.CurrentPage);
        msg.Headers.AddOrReplace("Referer", _pageNavigator.PrevPage.OriginalString, true);

        doc.LoadHtml(_client.HttpRequest(msg));

        var usersUrls = doc.DocumentNode
            .SelectNodes(xPathUserUrl)
            .Select(hn => new Uri($"{_pageNavigator.CurrentPage.Scheme}://{_pageNavigator.CurrentPage.Host}{hn.GetAttributeValue("href", default(string))}"));

        Thread.Sleep(_parserSetting.TimeoutAfterRequestToOneNumberMainPageWithUsers);

        return usersUrls;
    }

    private UserData ParseInfoAboutUser(Uri userUrl)
    {
        var doc = new HtmlDocument();

        var xPathFullName = "//h1[contains(@itemprop, 'name')]";
        var xPathSpecialtyAndCity = "//div[contains(@class, 'status')]/descendant::a[@href!='' and text()!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, userUrl);
        msg.Headers.AddOrReplace("Referer", _pageNavigator.CurrentPage.OriginalString, true);

        doc.LoadHtml(_client.HttpRequest(msg));

        var user = new UserData(doc.DocumentNode.SelectSingleNode(xPathFullName).InnerText, userUrl);
        user.ExtractSpecialtyAndCity(doc.DocumentNode.SelectSingleNode(xPathSpecialtyAndCity).InnerText);
        var contactsId = Regex.Match(doc.Text, @"(?<='spec_id_new_ppp',').*?(?=')").Value;

        Thread.Sleep(_parserSetting.TimeoutAfterRequestToOneUserPage);

        return user with { Contacts = this.ParseUserСontacts(userUrl, contactsId) };
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
            RequestUri = new Uri($"{_pageNavigator.CurrentPage.Scheme}://{_pageNavigator.CurrentPage.Host}/telefon_backend.php?mod=spec_id_new&id={contactsId}")
        };
        msg.Headers.AddRange(new Dictionary<string, string>
        {
            { "Referer", userLink.OriginalString },
            { "X-Requested-With", "XMLHttpRequest" }
        },
        true);

        var resp = _client.HttpRequest(msg);
        var contactsFromBackend = JsonSerializer.Deserialize<ContactsBackendModel>(resp) ?? new();

        doc.LoadHtml(contactsFromBackend.HtmlWithKontaktOne);

        var messengersUnderPhone = doc.DocumentNode
            .SelectNodes(xPathMessengersUnderPhone)
            ?.Select(x => x.InnerText) ?? Array.Empty<string>();

        var contacts = new UserContactsData
        {
            Phone = doc.DocumentNode?.SelectSingleNode(xPathPhone)?.InnerHtml ?? string.Empty,
            TelegramAvailable = messengersUnderPhone.Any(m => m.Contains("Telegram", StringComparison.OrdinalIgnoreCase)),
            WhatsAppAvailable = messengersUnderPhone.Any(m => m.Contains("WhatsApp", StringComparison.OrdinalIgnoreCase)),
            ViberAvailable = messengersUnderPhone.Any(m => m.Contains("Viber", StringComparison.OrdinalIgnoreCase)),

            TelegramUrl = ExtractHref(doc, "//a[contains(@href, 'tg')]"),
            VkUrl = ExtractHref(doc, "//a[contains(@href, 'vk.com')]"),
            YouTubeUrl = ExtractHref(doc, "//a[contains(@href, 'youtube.com')]"),
            SkypeNickname = ExtractHref(doc, "//a[contains(@href, 'Skype')]"),
            SiteUrl = ExtractHref(doc, "//a[contains(@href, 'http') and not(.//span)]")
        };

        Thread.Sleep(_parserSetting.TimeoutAfterRequestToOneUserPage);

        return contacts;
    }

    private static string ExtractHref(HtmlDocument doc, string xPath) =>
        doc.DocumentNode?.SelectSingleNode(xPath)?.GetAttributeValue("href", string.Empty) ?? string.Empty;
}