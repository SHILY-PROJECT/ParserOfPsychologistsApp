namespace ParserOfPsychologists.Application.Parser;

public class MainParser : IParser
{
    private readonly Random _rnd = new();

    private readonly IKeeperOfResult _keeperOfResult;
    private readonly IParserSettings _parserSetting;
    private readonly IPageNavigator _pageNavigator;
    private readonly HttpClient _client;

    private StateOfProgressEventArgs _progress = null!;

    public event EventHandler<StateOfProgressEventArgs>? StateOfProgressChanged;

    public MainParser(IKeeperOfResult keeperOfResult, IParserSettings parserSetting, IPageNavigator pageNavigator, HttpClient client)
    {
        _keeperOfResult = keeperOfResult;
        _parserSetting = parserSetting;
        _pageNavigator = pageNavigator;
        _client = client;
    }

    public async Task<IEnumerable<UserModel>> ParseUsersAsync() =>
        await Task.Run(() => ParseUsers());

    public IEnumerable<UserModel> ParseUsers()
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

        if (string.IsNullOrWhiteSpace(contactsId)) throw new ArgumentException("Parameter cannot be empty or null.", nameof(contactsId));
        if (string.IsNullOrWhiteSpace(userPageReferer.OriginalString)) throw new ArgumentException("Parameter cannot be empty or null.", nameof(userPageReferer));

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

        var socialMedia = new UserModel
        {
            TelegramUrl = ExtractHref(doc, "//a[contains(@href, 'tg')]"),
            VkUrl = ExtractHref(doc, "//a[contains(@href, 'vk.com')]"),
            YouTubeUrl = ExtractHref(doc, "//a[contains(@href, 'youtube.com')]"),
            SkypeNickname = ExtractHref(doc, "//a[contains(@href, 'Skype')]"),
            SiteUrl = ExtractHref(doc, "//a[contains(@href, 'http') and not(.//span)]")
        };

        var xPhone = "//span[contains(@style, 'text-decoration') and text()!='']";
        var xPhoneContainer = "//div/descendant::span[contains(@style, 'text-decoration') and text()!='']/ancestor::div[contains(@style, 'margin')][1]";
        var nodes = doc.DocumentNode?.SelectNodes(xPhoneContainer)?.Where(x => x is not null);

        if (nodes != null && nodes.Any())
        {
            foreach (var node in nodes)
            {
                var messengersUnderPhone = HttpUtility.HtmlDecode(node.InnerText).Trim() ?? string.Empty;
                if (node?.SelectSingleNode(xPhone)?.InnerText is not string phone || string.IsNullOrWhiteSpace(phone)) continue;

                contactsCollection.Add(new UserModel
                {
                    Phone = phone,
                    SmsAvailable = messengersUnderPhone.Contains("SMS", StringComparison.OrdinalIgnoreCase),
                    TelegramAvailable = messengersUnderPhone.Contains("Telegram", StringComparison.OrdinalIgnoreCase),
                    WhatsAppAvailable = messengersUnderPhone.Contains("WhatsApp", StringComparison.OrdinalIgnoreCase),
                    ViberAvailable = messengersUnderPhone.Contains("Viber", StringComparison.OrdinalIgnoreCase),
                });
            }
        }

        if (!contactsCollection.Any()) contactsCollection.Add(socialMedia);
        else contactsCollection = contactsCollection.Select(c => c with
        {
            TelegramUrl = socialMedia.TelegramUrl,
            VkUrl = socialMedia.VkUrl,
            YouTubeUrl = socialMedia.YouTubeUrl,
            SkypeNickname = socialMedia.SkypeNickname,
            SiteUrl = socialMedia.SiteUrl
        })
        .ToList();

        return contactsCollection;
    }

    private static string ExtractHref(HtmlDocument doc, string xPath) =>
        doc.DocumentNode?.SelectSingleNode(xPath)?.GetAttributeValue("href", string.Empty) ?? string.Empty; 
}