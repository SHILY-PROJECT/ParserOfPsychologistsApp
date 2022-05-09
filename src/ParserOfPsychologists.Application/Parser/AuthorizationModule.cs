namespace ParserOfPsychologists.Application.Authorization;

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
         *  TODO: Добавить авторизацию.
         */
        throw new NotImplementedException();
    }
}