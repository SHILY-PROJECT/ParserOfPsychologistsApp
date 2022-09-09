using System.Net.Http.Headers;
using System.Text;
using ParserOfPsychologists.Application.Interfaces;

namespace ParserOfPsychologists.Application.Toolkit;

public static class HttpHelper
{
    private static readonly HashSet<string> _errors = new(StringComparer.OrdinalIgnoreCase);

    public static void AddErrorsForVerifyContent(string[] errors) =>
        Array.ForEach(errors, err => _errors.Add(err));

    public static void AddRange(this HttpRequestHeaders requestHeaders, Dictionary<string, string> header, bool skipIfValueEmptyOrNull = false)
    {
        foreach (var kv in header)
        {
            if (skipIfValueEmptyOrNull && string.IsNullOrWhiteSpace(kv.Value)) continue;
            requestHeaders.Add(kv.Key, kv.Value);
        }
    }

    public static void AddRangeOrReplace(this HttpRequestHeaders requestHeaders, Dictionary<string, string> headers, bool skipIfValueEmptyOrNull = false)
    {
        foreach (var kv in headers)
        {
            if (skipIfValueEmptyOrNull && string.IsNullOrWhiteSpace(kv.Value)) continue;
            if (requestHeaders.Contains(kv.Key)) requestHeaders.Remove(kv.Key);
            requestHeaders.Add(kv.Key, kv.Value);
        }
    }

    public static void AddOrReplace(this HttpRequestHeaders requestHeaders, string name, string value, bool skipIfValueEmptyOrNull = false)
    {
        if (skipIfValueEmptyOrNull && string.IsNullOrWhiteSpace(value)) return;
        if (requestHeaders.Contains(name)) requestHeaders.Remove(name);
        requestHeaders.Add(name, value);
    }

    public static HttpClient CreateHttpClient(IHttpClientConfiguration httpClientConfiguration)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var client = new HttpClient(httpClientConfiguration.HttpClientHandler, false);
        client.DefaultRequestHeaders.AddRange(httpClientConfiguration.DefaultHeaders);
        return client;
    }

    public static async Task<string> HttpRequestAsync(this HttpClient client, HttpRequestMessage httpRequest, bool verifyResponse = false)
    {
        var resp = await client.SendAsync(httpRequest);
        using var streamReader = new StreamReader(await resp.Content.ReadAsStreamAsync(), Encoding.GetEncoding("windows-1251"));
        var content = streamReader.ReadToEnd();
        return !verifyResponse ? content : VerifyContent(content);
    }

    public static string HttpRequest(this HttpClient client, HttpRequestMessage httpRequest, bool verifyResponse = false)
    {
        var resp = client.Send(httpRequest);
        using var streamReader = new StreamReader(resp.Content.ReadAsStream(), Encoding.GetEncoding("windows-1251"));
        var content = streamReader.ReadToEnd();
        return !verifyResponse ? content : VerifyContent(content);
    }

    public static async Task<string> ExtractAsStringAsync(this HttpContent content)
    {
        using var streamReader = new StreamReader(await content.ReadAsStreamAsync(), Encoding.GetEncoding("windows-1251"));
        return await streamReader.ReadToEndAsync();
    }

    public static string ExtractAsString(this HttpContent content)
    {
        using var streamReader = new StreamReader(content.ReadAsStream(), Encoding.GetEncoding("windows-1251"));
        return streamReader.ReadToEnd();
    }

    public static string VerifyContent(HttpContent content) =>
        VerifyContent(content.ExtractAsString());
    
    public static string VerifyContent(string content)
    {
        foreach (var err in _errors)
        {
            if (content.Contains(err, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(err);
            }
        }
        return content;
    }
}