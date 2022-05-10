namespace ParserOfPsychologists.Application.Interfaces;

public interface ICityHandlerModule
{
    public event EventHandler<StateOfCityEventArgs>? CityChanged;

    int NumberOfPagesAvailable { get; }
    Uri CityUrl { get; }
    Dictionary<string, string> Cities { get; }
    Dictionary<string, string> DefaultCities { get; }

    bool IsChanged(string cityName);
    Task<IEnumerable<string>> FindCityAsync(string cityName);
    Task ChangeCityAsync(string cityName);
}