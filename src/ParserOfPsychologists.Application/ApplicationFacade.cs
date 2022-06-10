namespace ParserOfPsychologists.Application;

public class ApplicationFacade : IApplicationFacade
{
    private readonly IParser _parser;
    private readonly IParserSettings _parserSettings;
    private readonly IKeeperOfResult _keeperOfResult;
    private readonly ICityHandlerModule _cityHandler;
    private readonly IAuthorization _authorization;
    private readonly IAccountManager _accountManager;

    public event EventHandler<ApplicationInfoEventArgs>? ApplicationInfoSender;

    public ApplicationFacade(
        IKeeperOfResult keeperOfResult,
        IParser parser,
        IParserSettings parserSettings,
        ICityHandlerModule cityHandlerModule,
        IAuthorization authorization,
        IAccountManager accountManager)
    {
        _keeperOfResult = keeperOfResult;
        _parser = parser;
        _parserSettings = parserSettings;
        _cityHandler = cityHandlerModule;
        _authorization = authorization;
        _accountManager = accountManager;
    }

    public IParser Parser { get => _parser; }
    public IParserSettings ParserSettings { get => _parserSettings; }
    public IAuthorization Authorization { get => _authorization; }
    public ICityHandlerModule CityHandler { get => _cityHandler; }
    public IKeeperOfResult KeeperOfResult { get => _keeperOfResult; }
    public IAccountManager AccountManager { get => _accountManager; }

    public void OpenResultsFolder() =>
        _keeperOfResult.OpenResultsFolder();

    public async Task<bool> ConnectAccountAsync()
    {
        try
        {
            await _accountManager.LoadAccountProfileAsync();
        }
        catch
        {
            return false;
        }
        return true;
    }

    public async Task ChangeCityAsync()
    {
        try
        {
            await _cityHandler.ChangeCityAsync(_parserSettings.CityOnInput);
        }
        catch (Exception ex)
        {
            ApplicationInfoSender?.Invoke(this, new(ex.Message));
        }
    }

    public async Task<IDictionary<string, string>> GetDefaultCities()
    {
        IDictionary<string, string> cities = null!;

        try
        {
            cities = await _cityHandler.GetDefaultCities();
        }
        catch (Exception ex)
        {
            ApplicationInfoSender?.Invoke(this, new(ex.Message));
        }
     
        return cities ?? new Dictionary<string, string>();
    }

    public async Task<IDictionary<string, string>> FindCityAsync(string cityName)
    {
        IDictionary<string, string> cities = null!;

        try
        {
            cities = await _cityHandler.FindCityAsync(cityName);
        }
        catch (Exception ex)
        {
            ApplicationInfoSender?.Invoke(this, new(ex.Message));
        }
        
        return cities ?? new Dictionary<string, string>();
    }

    public async Task ParseUsersByCityAsync()
    {
        try
        {
            await _parser.ParseUsersAsync();            
        }
        catch (Exception ex)
        {
            ApplicationInfoSender?.Invoke(this, new(ex.Message));
        }
        finally
        {
            await _keeperOfResult.SaveToFileAsync();
        }
    }
}