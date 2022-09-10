using HtmlAgilityPack;
using ParserOfPsychologists.Application.CustomEventArgs;
using ParserOfPsychologists.Application.Interfaces;
using ParserOfPsychologists.Application.Models.ResponseBackend;
using ParserOfPsychologists.Application.Toolkit;
using System.Text.Json;
using System.Text;

namespace ParserOfPsychologists.Application.B17.Cities;

public class CityProvider
{
    private static string MainUrl => B17Data.MainUrl;
    
    private readonly HtmlDocument _doc;
    private readonly HttpClient _http;

    private string _prevCity = MainUrl;
    private string _cityChangeKey = string.Empty;

    public CityProvider()
    {
        _doc = new HtmlDocument();
        _http = HttpClientFactory.CreateDefautlHttpClient();
        _ = this.SetDefaultCities();
    }

    public List<CityQueryModel> Cities { get; } = new();
    
    public async Task<List<CityQueryModel>> FindCityAsync(string searchQueryText)
    {
        var xCityName = "//div[@city!='' and text()!='']";
        var xCityChangeKey = "//input[contains(@id, 'city_change_key')]";

        if (Cities.Where(c => c.SearchQueryText.Equals(searchQueryText, StringComparison.OrdinalIgnoreCase)).ToList() is List<CityQueryModel> cities && cities.Any())
        {
            return cities;
        }

        var msg = new HttpRequestMessage(HttpMethod.Post, $"{MainUrl}/city_backend.php")
        {
            Content = new StringContent($"mod=backend&city_text={searchQueryText}", Encoding.UTF8, "application/x-www-form-urlencoded"),
        };
        msg.Headers.AddOrReplace("Connection", "keep-alive");
        
        var resp = await _http.HttpRequestAsync(msg);
        var cityBackend = JsonSerializer.Deserialize<CityBackendModel>(resp) ?? new();
        _doc.LoadHtml(cityBackend.Body);

        _cityChangeKey = _doc.DocumentNode?.SelectSingleNode(xCityChangeKey)?.GetAttributeValue("value", string.Empty) ?? string.Empty;

        var updatedCities = _doc.DocumentNode
            ?.SelectNodes(xCityName)
            ?.Select(hn => new CityQueryModel(hn.InnerHtml, hn.GetAttributeValue("city", string.Empty)))
            ?.ToList();
        
        if (updatedCities is not null) Cities.AddRange(updatedCities);

        return updatedCities ?? new List<CityQueryModel>();
    }

    public async Task SelectCityAsync(CityQueryModel cityQueryModel)
    {
        if (cityQueryModel.AvailablePages != -1 && !string.IsNullOrWhiteSpace(cityQueryModel.Route)) return;

        var url = $"{MainUrl}/city_backend.php?action=city_change&city={cityQueryModel.Param}&city_change_key={_cityChangeKey}";
        var msg = new HttpRequestMessage(HttpMethod.Get, url);
        msg.Headers.AddOrReplace("Referer", _prevCity);

        var resp = await _http.SendAsync(msg);
        HttpHelper.VerifyContent(resp.Content);

        if (resp.Headers.GetValues("Location").FirstOrDefault() is string valueRoute)
        {
            cityQueryModel.Route = valueRoute;
            cityQueryModel.AvailablePages = await this.GetAvailablePages(MainUrl + cityQueryModel.Route);
        }
    }

    private async Task SetDefaultCities() =>
        await this.FindCityAsync(string.Empty);

    private async Task<int> GetAvailablePages(string url)
    {
        var value = 0;
        var xLastPage = "//div[contains(@id, 'page_list')]/descendant::a[last()]";
        var xUserLink = "//div[contains(@id, 'items_list_main')]/descendant::a[contains(@name, 'spec') and @href!='']";

        var msg = new HttpRequestMessage(HttpMethod.Get, url);
        msg.Headers.AddOrReplace("Referer", _prevCity);

        var resp = await _http.HttpRequestAsync(msg, true);
        _doc.LoadHtml(resp);
        _prevCity = url;

        return value switch
        {
            _ when int.TryParse(_doc.DocumentNode?.SelectSingleNode(xLastPage)?.InnerText, out value) => value,
            _ when _doc.DocumentNode?.SelectSingleNode(xUserLink) != null => 1,
            _ => 0
        };
    }
}