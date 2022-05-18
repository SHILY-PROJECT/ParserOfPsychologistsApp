namespace ParserOfPsychologists.Application.Parser;

public class AuthorizationModule : IAuthorization
{
    private readonly IParserSettings _parserSettings;
    private readonly HttpClient _client;
    private readonly AccountData _account;

    private string _capthaId = string.Empty;
    private string _capthaIdKey = string.Empty;
    private string _capthaImg = string.Empty;

    public AuthorizationModule(IParserSettings parserSettings, HttpClient client, AccountData account)
    {
        _parserSettings = parserSettings;
        _client = client;
        _account = account;
    }

    public Task<bool> SignInAsync()
    {
        
        /*
         *  TODO: Add authorization.
         */
        throw new NotImplementedException();
    }

    public async Task<Image?> GetCaptcha()
    {
        var xCaptchaId = "//input[@name='captha_id']";
        var xCaptchaIdKey = "//input[@name='captha_id_key']";
        var xCaptchaImg = "//form[contains(@action, 'login')]/descendant::img[@src!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, $"{_parserSettings.MainUrl}/login.php");
        msg.Headers.AddOrReplace("Connection", "keep-alive");

        var doc = new HtmlDocument();
        var resp = await _client.HttpRequestAsync(msg);
        doc.LoadHtml(resp);

        _capthaId = doc.DocumentNode.SelectSingleNode(xCaptchaId).GetAttributeValue("value", string.Empty);
        _capthaIdKey = doc.DocumentNode.SelectSingleNode(xCaptchaIdKey).GetAttributeValue("value", string.Empty);
        _capthaImg = doc.DocumentNode.SelectSingleNode(xCaptchaImg).GetAttributeValue("src", string.Empty);

        var rx = new Regex(@"data:image/(?<type>.+?),(?<data>.+)");

        if (rx.IsMatch(_capthaImg) && rx.Match(_capthaImg).Groups["data"].Value is string value)
        {
            using var stream = new MemoryStream(Convert.FromBase64String(value));
            return Image.FromStream(stream);
        }
        else return null;
    }
}