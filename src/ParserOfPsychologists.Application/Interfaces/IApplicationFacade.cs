namespace ParserOfPsychologists.Application.Interfaces;

public interface IApplicationFacade
{
    IParserSettings ParserSettings { get; }
    ICityHandlerModule CityHandler { get; }

    Task ChangeCityAsync();
    Task<bool> ConnectAnAccountAsync();
    Task<IReadOnlyCollection<string>> FindCityAsync(string cityName);
    Task<IReadOnlyCollection<UserData>> ParseUsersByCityAsync();
}