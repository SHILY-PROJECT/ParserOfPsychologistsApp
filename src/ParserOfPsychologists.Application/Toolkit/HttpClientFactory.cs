using ParserOfPsychologists.Application.Models;
using System.Net;
using System.Text;

namespace ParserOfPsychologists.Application.Toolkit;

public class HttpClientFactory
{
    public static HttpClient CreateHttpClientForAccount(AccountModule account, bool allowAutoRedirect = false) =>
        CreateHttpClient(account.CookieContainer, allowAutoRedirect);

    public static HttpClient CreateDefautlHttpClient(bool allowAutoRedirect = false) =>
        CreateHttpClient(new CookieContainer(), allowAutoRedirect);

    public static HttpClient CreateHttpClient(CookieContainer cookieContainer, bool allowAutoRedirect = false)
    {
        var httpClientHandler = new HttpClientHandler()
        {
            AllowAutoRedirect = allowAutoRedirect,
            MaxAutomaticRedirections = 5,
            AutomaticDecompression = DecompressionMethods.All,
            CookieContainer = cookieContainer
        };

        var defaultHeaders = new Dictionary<string, string>
        {
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8" },
            { "Accept-Encoding", "gzip, deflate, br" },
            { "Accept-Language", "en-US,en;q=0.9,ru;q=0.8" },
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.75 Safari/537.36" },
            { "Connection", "keep-alive" }
        };

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var client = new HttpClient(httpClientHandler, false);
        client.DefaultRequestHeaders.AddRange(defaultHeaders);

        return client;
    }
}
