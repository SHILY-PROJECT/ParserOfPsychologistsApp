﻿namespace ParserOfPsychologists.Application.Parser;

public class StateOfCityModule : IStateOfCityModule
{
    private readonly string _mainUrl = "https://www.b17.ru";

    private string _cityName = string.Empty;
    private string _cityRoute = string.Empty;
    private string _cityChangeKey = string.Empty;

    private readonly HttpClient _client;

    public StateOfCityModule(HttpClient client)
    {
        _client = client;
    }

    public Uri CityUrl { get => new($"{_mainUrl}{_cityRoute}"); }
    public Dictionary<string, string> DefaultCities { get; } = new();
    public Dictionary<string, string> Cities { get; } = new();

    public bool IsChanged(string cityName) =>
        !string.IsNullOrWhiteSpace(cityName) && !_cityName.Equals(cityName, StringComparison.OrdinalIgnoreCase);

    public async Task<IEnumerable<string>> FindCityAsync(string cityName)
    {
        var doc = new HtmlDocument();

        var xPathCityChangeKey = "//input[contains(@id, 'city_change_key')]";
        var xPathCity = "//div[@city!='' and text()!='']";

        var msg = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_mainUrl}/city_backend.php"),
            Content = new StringContent($"mod=backend&city_text={cityName}"),
        };
        msg.Headers.Add("Connection", "keep-alive");
        msg.Content.Headers.ContentType = new("application/x-www-form-urlencoded");
        msg.Content.Headers.ContentEncoding.Add("UTF-8");

        var resp = await _client.HttpRequestAsync(msg);
        var cityBackend = JsonSerializer.Deserialize<CityBackendModel>(resp) ?? new();
        doc.LoadHtml(cityBackend.Body);

        _cityChangeKey = doc.DocumentNode?.SelectSingleNode(xPathCityChangeKey)?.GetAttributeValue("value", string.Empty) ?? string.Empty;
        doc.DocumentNode?.SelectNodes(xPathCity).ToList().ForEach(hn => Cities.Add(hn.InnerHtml, hn.GetAttributeValue("city", string.Empty)));

        return Cities.Select(kv => kv.Key);
    }

    public async Task ChangeCityAsync(string cityName)
    {
        var msg = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_mainUrl}/city_backend.php?action=city_change&city={Cities[cityName]}&city_change_key={_cityChangeKey}")
        };
        msg.Headers.AddOrReplace("Referer", CityUrl.OriginalString);

        var resp = await _client.SendAsync(msg);

        if (resp.Headers.GetValues("Location").FirstOrDefault() is string value)
        {
            _cityRoute = value;
            _cityName = cityName;
        }
    }
}