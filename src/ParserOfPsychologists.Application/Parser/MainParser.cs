namespace ParserOfPsychologists.Application.Parser;

public class MainParser : IParser
{
    private readonly Random _rnd = new();

    private readonly KeeperOfResult _keeperOfResult;
    private readonly IParserSettings _parserSetting;
    private readonly IPageNavigator _pageNavigator;
    private readonly HttpClient _client;

    private StateOfProgressEventArgs _progress = null!;

    public event EventHandler<StateOfProgressEventArgs>? StateOfProgressChanged;

    public MainParser(KeeperOfResult keeperOfResult, IParserSettings parserSetting, IPageNavigator pageNavigator, HttpClient client)
    {
        _keeperOfResult = keeperOfResult;
        _parserSetting = parserSetting;
        _pageNavigator = pageNavigator;
        _client = client;
    }

    public async Task<IEnumerable<UserModel>> ParseUsersByCityAsync() =>
        await Task.Run(() => ParseUsersByCity());

    private IEnumerable<UserModel> ParseUsersByCity()
    {
        _keeperOfResult.Users = new List<UserModel>();
        _progress = new StateOfProgressEventArgs();

        while (_pageNavigator.MoveNextOnPage())
        {
            var userLinks = this.ParseUsersFromPage().OrderBy(u => _rnd.Next());

            foreach (var user in userLinks.Select(ul => this.GetUserPage(ul)).SelectMany(u => u))
            {
                _keeperOfResult.Users.Add(user);
            }

            ++_progress.NumberOfPagesProcessed;
            OnStateOfProgressChanged();
        }

        return _keeperOfResult.Users;
    }

    protected void OnStateOfProgressChanged() =>
        StateOfProgressChanged?.Invoke(this, _progress);

    private IEnumerable<Uri> ParseUsersFromPage()
    {
        var doc = new HtmlDocument();

        var xPathUserUrl = "//div[contains(@id, 'items_list_main')]/descendant::a[contains(@name, 'spec') and @href!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, _pageNavigator.CurrentPage);
        msg.Headers.AddOrReplace("Referer", _pageNavigator.PrevPage.OriginalString, true);

        doc.LoadHtml(_client.HttpRequest(msg, true));

        var usersUrls = doc.DocumentNode
            .SelectNodes(xPathUserUrl)
            .Select(hn => new Uri($"{_pageNavigator.CurrentPage.Scheme}://{_pageNavigator.CurrentPage.Host}{hn.GetAttributeValue("href", default(string))}"));

        Thread.Sleep(_parserSetting.TimeoutAfterRequestToOneNumberMainPageWithUsers);

        return usersUrls;
    }

    private IList<UserModel> GetUserPage(Uri userPage)
    {
        var doc = new HtmlDocument();
        var users = new List<UserModel>();

        var xPathFullName = "//h1[contains(@itemprop, 'name')]";
        var xPathSpecialtyAndCity = "//div[contains(@class, 'status')]/descendant::a[@href!='' and text()!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, userPage);
        msg.Headers.AddOrReplace("Referer", _pageNavigator.CurrentPage.OriginalString, true);

        doc.LoadHtml(_client.HttpRequest(msg, true));

        var user = new UserModel
        {
            FullName = doc.DocumentNode.SelectSingleNode(xPathFullName).InnerText,
            UrlOnSite = userPage.OriginalString
        };
        user.ExtractSpecialtyAndCity(doc.DocumentNode.SelectSingleNode(xPathSpecialtyAndCity).InnerText);
        var contactsId = Regex.Match(doc.Text, @"(?<='spec_id_new_ppp',').*?(?=')").Value;

        Thread.Sleep(_parserSetting.TimeoutAfterRequestToOneUserPage);

        foreach (var contacts in this.GetUserСontacts(contactsId, userPage))
        {
            users.Add(user with
            {
                Phone = contacts.Phone,
                SmsAvailable = contacts.SmsAvailable,
                TelegramAvailable = contacts.TelegramAvailable,
                WhatsAppAvailable = contacts.WhatsAppAvailable,
                ViberAvailable = contacts.ViberAvailable,

                TelegramUrl = contacts.TelegramUrl,
                VkUrl = contacts.VkUrl,
                YouTubeUrl = contacts.YouTubeUrl,
                SkypeNickname = contacts.SkypeNickname,
                SiteUrl = contacts.SiteUrl
            });
        }

        ++_progress.NumberOfUsersProcessed;
        OnStateOfProgressChanged();

        Thread.Sleep(_parserSetting.TimeoutAfterRequestToOneUserPage);

        return users;
    }

    private IList<UserModel> GetUserСontacts(string contactsId, Uri userPageReferer)
    {
        var doc = new HtmlDocument();
        var contactsCollection = new List<UserModel>();

        var xPathPhone = "//div/descendant::span[contains(@style, 'text-decoration') and text()!='']";
        var xPathAvailablePhoneCommunication = "./../following-sibling::div/descendant::span[contains(@style, 'contact') and text()!='']/parent::div[text()!='']|./../following-sibling::div[text()!='']";
        
        if (string.IsNullOrWhiteSpace(userPageReferer.OriginalString))
            throw new ArgumentException("Parameter cannot be empty or null.", nameof(userPageReferer));
        if (string.IsNullOrWhiteSpace(contactsId))
            throw new ArgumentException("Parameter cannot be empty or null.", nameof(contactsId));

        var msg = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_pageNavigator.CurrentPage.Scheme}://{_pageNavigator.CurrentPage.Host}/telefon_backend.php?mod=spec_id_new&id={contactsId}")
        };
        msg.Headers.AddRange(new Dictionary<string, string>
        {
            { "Referer", userPageReferer.OriginalString },
            { "X-Requested-With", "XMLHttpRequest" }
        },
        true);

        var resp = _client.HttpRequest(msg, true);
        var contactsFromBackend = JsonSerializer.Deserialize<ContactsBackendModel>(resp) ?? new();

        doc.LoadHtml(contactsFromBackend.HtmlWithKontaktOne);
        var phoneNodes = doc.DocumentNode?.SelectNodes(xPathPhone);

        var socialMedia = new UserModel
        {
            TelegramUrl = ExtractHref(doc, "//a[contains(@href, 'tg')]"),
            VkUrl = ExtractHref(doc, "//a[contains(@href, 'vk.com')]"),
            YouTubeUrl = ExtractHref(doc, "//a[contains(@href, 'youtube.com')]"),
            SkypeNickname = ExtractHref(doc, "//a[contains(@href, 'Skype')]"),
            SiteUrl = ExtractHref(doc, "//a[contains(@href, 'http') and not(.//span)]")
        };

        if (phoneNodes is null)
        {
            contactsCollection.Add(socialMedia);
            return contactsCollection;
        }

        foreach (var node in phoneNodes)
        {
            var messengersUnderPhone = node
                ?.SelectSingleNode(xPathAvailablePhoneCommunication)
                ?.SelectNodes(".//text()")
                ?.Select(x => x?.InnerText ?? "")
                ?.ToArray() ?? Array.Empty<string>();

            if (node?.InnerText is string phone && !string.IsNullOrWhiteSpace(phone)) contactsCollection.Add(new UserModel
            {
                Phone = phone,
                SmsAvailable = messengersUnderPhone.Any(m => m.Contains("SMS", StringComparison.OrdinalIgnoreCase)),
                TelegramAvailable = messengersUnderPhone.Any(m => m.Contains("Telegram", StringComparison.OrdinalIgnoreCase)),
                WhatsAppAvailable = messengersUnderPhone.Any(m => m.Contains("WhatsApp", StringComparison.OrdinalIgnoreCase)),
                ViberAvailable = messengersUnderPhone.Any(m => m.Contains("Viber", StringComparison.OrdinalIgnoreCase)),
            });
        }

        return contactsCollection.Select(c => c with
        {
            TelegramUrl = socialMedia.TelegramUrl,
            VkUrl = socialMedia.VkUrl,
            YouTubeUrl = socialMedia.YouTubeUrl,
            SkypeNickname = socialMedia.SkypeNickname,
            SiteUrl = socialMedia.SiteUrl
        })
        .ToList();
    }

    private static string ExtractHref(HtmlDocument doc, string xPath) =>
        doc.DocumentNode?.SelectSingleNode(xPath)?.GetAttributeValue("href", string.Empty) ?? string.Empty; 
}