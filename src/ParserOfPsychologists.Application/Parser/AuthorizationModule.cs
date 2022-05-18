namespace ParserOfPsychologists.Application.Parser;

public class AuthorizationModule
{
    private readonly HttpClient _client;

    public AuthorizationModule(HttpClient client)
    {
        _client = client;
    }

    public Task<bool> SignInAsync(AccountData account)
    {
        /*
         *  TODO: Add authorization.
         */
        throw new NotImplementedException();
    }
}