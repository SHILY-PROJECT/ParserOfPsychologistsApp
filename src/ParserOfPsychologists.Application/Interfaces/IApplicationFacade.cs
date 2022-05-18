using ParserOfPsychologists.Application.Parser.CustomEventArgs;

namespace ParserOfPsychologists.Application.Interfaces;

public interface IApplicationFacade
{
    event EventHandler<ApplicationInfoEventArgs>? ApplicationInfoSender;

    IParser Parser { get; }
    IParserSettings ParserSettings { get; }
    ICityHandlerModule CityHandler { get; }

    void OpenResultsFolder();

    Task<IDictionary<string, string>> GetDefaultCities();
    Task ChangeCityAsync();
    Task<bool> ConnectAnAccountAsync();
    Task<IDictionary<string, string>> FindCityAsync(string cityName);
    Task ParseUsersByCityAsync();
}