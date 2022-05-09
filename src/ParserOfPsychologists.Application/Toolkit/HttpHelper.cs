namespace ParserOfPsychologists.Application.Toolkit;

public static class HttpHelper
{
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

    public static async Task<string> HttpRequestAsync(this HttpClient client, HttpRequestMessage httpRequest)
    {
        var resp = await client.SendAsync(httpRequest);
        using var streamReader = new StreamReader(await resp.Content.ReadAsStreamAsync(), Encoding.GetEncoding("windows-1251"));
        return await streamReader.ReadToEndAsync();
    }

    public static async Task<string> ExtractAsString(this HttpContent content)
    {
        using var streamReader = new StreamReader(await content.ReadAsStreamAsync(), Encoding.GetEncoding("windows-1251"));
        return await streamReader.ReadToEndAsync();
    }
}