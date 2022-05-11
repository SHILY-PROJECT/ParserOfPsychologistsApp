namespace ParserOfPsychologists.Application;

public class ApplicationFacade : IApplicationFacade
{
    private readonly IParser _parser;
    private readonly IParserSettings _parserSettings;
    private readonly ICityHandlerModule _cityHandler;
    private readonly IAccountManager _accountManager;

    public ApplicationFacade(
        IParser parser,
        IParserSettings parserSettings,
        ICityHandlerModule cityHandlerModule,
        IAccountManager accountManager)
    {
        _parser = parser;
        _parserSettings = parserSettings;
        _cityHandler = cityHandlerModule;
        _accountManager = accountManager;
    }

    public IParserSettings ParserSettings { get => _parserSettings; }
    public ICityHandlerModule CityHandler { get => _cityHandler; }

    public async Task<bool> ConnectAnAccountAsync()
    {
        try
        {
            await _accountManager.ConnectAnAccountAsync();
        }
        catch
        {
            return false;
        }
        return true;
    }

    public async Task ChangeCityAsync()
    {
        await _cityHandler.ChangeCityAsync(_parserSettings.CityOnInput);
    }

    public async Task<IReadOnlyCollection<string>> FindCityAsync(string cityName)
    {
        return (await _cityHandler.FindCityAsync(cityName)).ToArray();
    }

    public async Task<IReadOnlyCollection<UserData>> ParseUsersByCityAsync()
    {
        return (await _parser.ParseUsersByCityAsync()).ToList();
    }
}