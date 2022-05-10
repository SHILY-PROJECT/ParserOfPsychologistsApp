namespace ParserOfPsychologists.Application;

public class ParserWebFacade : IParserWebRequestsFacade
{
    private readonly IParser _parser;
    private readonly ICityHandlerModule _cityModule;
    private readonly IAccountManager _accountManager;

    public ParserWebFacade(IParser parser, ICityHandlerModule cityModule, IAccountManager accountManager)
    {
        _parser = parser;
        _cityModule = cityModule;
        _accountManager = accountManager;
    }

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

    public async Task<IReadOnlyCollection<string>> FindCityAsync(string cityName)
    {
        return (await _cityModule.FindCityAsync(cityName)).ToArray();
    }

    public async Task<IReadOnlyCollection<UserData>> ParseUsersByCityAsync()
    {
        return (await _parser.ParseUsersByCityAsync()).ToList();
    }
}