namespace ParserOfPsychologists.Application.Configuration;

public class HttpClientConfiguration : IHttpClientConfiguration
{
    private HttpClientConfiguration() { }

    public CookieContainer CookieContainer { get; private set; } = null!;
    public HttpClientHandler HttpClientHandler { get; private set; } = null!;
    public Dictionary<string, string> DefaultHeaders { get; private set; } = null!;

    public static HttpClientConfiguration CreateConfiguration()
    {
        var cfg = new HttpClientConfiguration();

        cfg.HttpClientHandler = new HttpClientHandler()
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.All,
            CookieContainer = cfg.CookieContainer = new()
        };
        cfg.DefaultHeaders = new()
        {
            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8" },
            { "Accept-Encoding", "gzip, deflate, br" },
            { "Accept-Language", "en-US,en;q=0.9,ru;q=0.8" },
            { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.75 Safari/537.36" },
            { "Connection", "keep-alive" }
        };

        return cfg;
    }
}