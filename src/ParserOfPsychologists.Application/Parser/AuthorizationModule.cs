using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using HtmlAgilityPack;
using ParserOfPsychologists.Application.Models;
using ParserOfPsychologists.Application.Toolkit;
using ParserOfPsychologists.Application.Interfaces;
using ParserOfPsychologists.Application.Configuration;

namespace ParserOfPsychologists.Application.Parser;

public class AuthorizationModule : IAuthorization
{
    private readonly IParserSettings _parserSettings;
    private readonly HttpClient _client;
    private readonly AccountData _account;
    private readonly CaptchaModel _captcha;
    private readonly HttpClientConfiguration _httpClientConfiguration;

    public AuthorizationModule(
        IParserSettings parserSettings,
        HttpClient client,
        AccountData account,
        CaptchaModel captcha,
        IHttpClientConfiguration httpClientConfiguration)
    {
        _parserSettings = parserSettings;
        _client = client;
        _account = account;
        _captcha = captcha;
        _httpClientConfiguration = (HttpClientConfiguration)httpClientConfiguration;
    }

    public async Task<bool> SignInAsync()
    {
        if (string.IsNullOrWhiteSpace(_account.Login)) throw new InvalidOperationException("Login cannot be empty.");
        if (string.IsNullOrWhiteSpace(_account.Password)) throw new InvalidOperationException("Password cannot be empty.");

        var xErrorBox = "//div[contains(@class, 'box-red')]";
        var xPrivatMessages = "//div[contains(@id, 'main')]/descendant::a[contains(@href, 'privat')]";

        var msg = new HttpRequestMessage(HttpMethod.Post, $"{_parserSettings.MainUrl}/login.php?action=login")
        {
            Content = new StringContent($"referer=&l_login={HttpUtility.UrlEncode(_account.Login)}&l_password={HttpUtility.UrlEncode(_account.Password)}&captha_id={_captcha.Id}&captha_id_key={_captcha.Key}&captha_text={_captcha.InputText}", Encoding.UTF8, "application/x-www-form-urlencoded")
        };
        msg.Headers.AddOrReplace("Referer", $"{_parserSettings.MainUrl}/login.php");

        _httpClientConfiguration.ChangeRedirect(true);
        var resp = await _client.HttpRequestAsync(msg);
        _httpClientConfiguration.ChangeRedirect(false);

        var doc = new HtmlDocument();
        doc.LoadHtml(resp);

        if (doc.DocumentNode?.SelectSingleNode(xPrivatMessages) is null) throw new InvalidOperationException(xErrorBox switch
        {
            _ when doc.DocumentNode?.SelectSingleNode(xErrorBox) is HtmlNode node => node.InnerText,
            _ => "Unknown authorization error."
        });

        return true;
    }

    public async Task<CaptchaModel> UpdateCaptcha()
    {
        var xCaptchaId = "//input[@name='captha_id']";
        var xCaptchaIdKey = "//input[@name='captha_id_key']";
        var xCaptchaImg = "//form[contains(@action, 'login')]/descendant::img[@src!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, $"{_parserSettings.MainUrl}/login.php");
        msg.Headers.AddOrReplace("Connection", "keep-alive");

        var doc = new HtmlDocument();
        var resp = await _client.HttpRequestAsync(msg);
        doc.LoadHtml(resp);

        _captcha.Id = doc.DocumentNode.SelectSingleNode(xCaptchaId).GetAttributeValue("value", string.Empty);
        _captcha.Key = doc.DocumentNode.SelectSingleNode(xCaptchaIdKey).GetAttributeValue("value", string.Empty);
        var _captchaSrcValue = doc.DocumentNode.SelectSingleNode(xCaptchaImg).GetAttributeValue("src", string.Empty);

        var rx = new Regex(@"data:image/(?<type>.+?),(?<data>.+)");

        if (!rx.IsMatch(_captchaSrcValue) || rx.Match(_captchaSrcValue).Groups["data"].Value is not string value)
            throw new InvalidOperationException("Something went wrong during the captcha update...");

        using var stream = new MemoryStream(Convert.FromBase64String(value));
        _captcha.Img = Image.FromStream(stream);
        
        return _captcha;
    }
}