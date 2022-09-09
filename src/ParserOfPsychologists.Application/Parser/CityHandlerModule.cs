using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using ParserOfPsychologists.Application.Models.ResponseBackend;
using ParserOfPsychologists.Application.Toolkit;
using ParserOfPsychologists.Application.Interfaces;
using ParserOfPsychologists.Application.CustomEventArgs;

namespace ParserOfPsychologists.Application.Parser;

public class CityHandlerModule : ICityHandlerModule
{
    private readonly Dictionary<string, string> _defaultCities = new();
    private readonly IParserSettings _parserSettings;
    private readonly HttpClient _client;

    private string _cityName = string.Empty;
    private string _cityRoute = string.Empty;
    private string _cityChangeKey = string.Empty;

    public event EventHandler<CityHandlerModuleEventArgs>? CityChanged;

    public CityHandlerModule(IParserSettings parserSettings, HttpClient client)
    {
        _parserSettings = parserSettings;
        _client = client;
    }

    public int NumberOfAvailablePagesForCurrentCity { get; private set; }
    public Uri CityUrl { get => new($"{_parserSettings.MainUrl}{_cityRoute}"); }
    public Dictionary<string, string> Cities { get; } = new();

    public void ResetCurrentCity()
    {
        (_cityName, _cityRoute) = (string.Empty, string.Empty);

        if (Cities.Any()) Cities.Clear();
        Cities.AddRange(_defaultCities);
    }

    public bool IsChanged(string cityName) =>
        !string.IsNullOrWhiteSpace(cityName) && !_cityName.Equals(cityName, StringComparison.OrdinalIgnoreCase);

    public async Task<IDictionary<string, string>> GetDefaultCities()
    {
        if (!_defaultCities.Any())
        {
            foreach (var city in await FindCityAsync(string.Empty))
            {
                _defaultCities.Add(city.Key, city.Value);
            }
        }
        return _defaultCities;
    }

    public async Task<IDictionary<string, string>> FindCityAsync(string cityName)
    {
        var xCityName = "//div[@city!='' and text()!='']";
        var xCityChangeKey = "//input[contains(@id, 'city_change_key')]";

        var msg = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_parserSettings.MainUrl}/city_backend.php"),
            Content = new StringContent($"mod=backend&city_text={cityName}", Encoding.UTF8, "application/x-www-form-urlencoded"),
        };
        msg.Headers.AddOrReplace("Connection", "keep-alive");

        var doc = new HtmlDocument();
        var resp = await _client.HttpRequestAsync(msg);
        var cityBackend = JsonSerializer.Deserialize<CityBackendModel>(resp) ?? new();
        doc.LoadHtml(cityBackend.Body);

        _cityChangeKey = doc.DocumentNode?.SelectSingleNode(xCityChangeKey)?.GetAttributeValue("value", string.Empty) ?? string.Empty;

        var updatedCities = doc.DocumentNode
            ?.SelectNodes(xCityName)
            ?.Select(hn => KeyValuePair.Create(hn.InnerHtml, hn.GetAttributeValue("city", string.Empty)))
            ?.ToDictionary(x => x.Key, x => x.Value);

        if (Cities.Any()) Cities.Clear();
        if (updatedCities != null) Cities.AddRange(updatedCities);

        return Cities;
    }

    public async Task ChangeCityAsync(string cityName)
    {
        var referer = CityUrl.OriginalString;
        var msg = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_parserSettings.MainUrl}/city_backend.php?action=city_change&city={Cities[cityName]}&city_change_key={_cityChangeKey}")
        };
        msg.Headers.AddOrReplace("Referer", referer);

        var resp = await _client.SendAsync(msg);
        HttpHelper.VerifyContent(resp.Content);

        if (resp.Headers.GetValues("Location").FirstOrDefault() is string value)
        {
            _cityRoute = value;
            _cityName = cityName;

            NumberOfAvailablePagesForCurrentCity = await this.GetAvailablePages(referer);
            this.OnCityChanged(new CityHandlerModuleEventArgs(NumberOfAvailablePagesForCurrentCity));
        }
    }

    protected void OnCityChanged(CityHandlerModuleEventArgs args) =>
        CityChanged?.Invoke(this, args);

    private async Task<int> GetAvailablePages(string referer)
    {
        var xLastPage = "//div[contains(@id, 'page_list')]/descendant::a[last()]";
        var xUserLink = "//div[contains(@id, 'items_list_main')]/descendant::a[contains(@name, 'spec') and @href!='']";

        var value = 0;

        var msg = new HttpRequestMessage(HttpMethod.Get, CityUrl);
        msg.Headers.AddOrReplace("Referer", referer);

        var doc = new HtmlDocument();
        var resp = await _client.HttpRequestAsync(msg, true);
        doc.LoadHtml(resp);

        return value switch
        {
            _ when int.TryParse(doc.DocumentNode?.SelectSingleNode(xLastPage)?.InnerText, out value) => value,
            _ when doc.DocumentNode?.SelectSingleNode(xUserLink) != null => 1,
            _ => 0
        };
    }
}