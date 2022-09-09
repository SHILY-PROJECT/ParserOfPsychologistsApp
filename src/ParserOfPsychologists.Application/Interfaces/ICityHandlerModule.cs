using ParserOfPsychologists.Application.CustomEventArgs;

namespace ParserOfPsychologists.Application.Interfaces;

public interface ICityHandlerModule
{
    event EventHandler<CityHandlerModuleEventArgs>? CityChanged;

    int NumberOfAvailablePagesForCurrentCity { get; }
    Uri CityUrl { get; }
    Dictionary<string, string> Cities { get; }

    void ResetCurrentCity();
    bool IsChanged(string cityName);

    Task<IDictionary<string, string>> GetDefaultCities();
    Task ChangeCityAsync(string cityName);
    Task<IDictionary<string, string>> FindCityAsync(string cityName);
}