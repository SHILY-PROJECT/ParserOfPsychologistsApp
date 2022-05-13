namespace ParserOfPsychologists.Application;

public class ApplicationFacade : IApplicationFacade
{
    private readonly IParser _parser;
    private readonly IParserSettings _parserSettings;
    private readonly ICityHandlerModule _cityHandler;
    private readonly IAccountManager _accountManager;
    private readonly KeeperOfResult _keeperOfResult;

    public event EventHandler<ApplicationInfoEventArgs>? ApplicationInfoSender;

    public ApplicationFacade(
        IParser parser,
        IParserSettings parserSettings,
        ICityHandlerModule cityHandlerModule,
        IAccountManager accountManager,
        KeeperOfResult keeperOfResult)
    {
        _parser = parser;
        _parserSettings = parserSettings;
        _cityHandler = cityHandlerModule;
        _accountManager = accountManager;
        _keeperOfResult = keeperOfResult;
    }

    public IParser Parser { get => _parser; }
    public IParserSettings ParserSettings { get => _parserSettings; }
    public ICityHandlerModule CityHandler { get => _cityHandler; }

    public void OpenResultsFolder() => _keeperOfResult.OpenResultsFolder();

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
            await _parser.ParseUsersByCityAsync();            
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