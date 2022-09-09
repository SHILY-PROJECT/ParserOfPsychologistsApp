namespace ParserOfPsychologists.Application.Interfaces;

public interface IApplicationFacade
{
    event EventHandler<ApplicationInfoEventArgs>? ApplicationInfoSender;

    IParser Parser { get; }
    IParserSettings ParserSettings { get; }
    IAuthorization Authorization { get; }
    ICityHandlerModule CityHandler { get; }
    IKeeperOfResult KeeperOfResult { get; }
    IAccountManager AccountManager { get; }

    void OpenResultsFolder();

    Task<IDictionary<string, string>> GetDefaultCities();
    Task ChangeCityAsync();
    Task<bool> ConnectAccountAsync();
    Task<IDictionary<string, string>> FindCityAsync(string cityName);
    Task ParseUsersByCityAsync();
}