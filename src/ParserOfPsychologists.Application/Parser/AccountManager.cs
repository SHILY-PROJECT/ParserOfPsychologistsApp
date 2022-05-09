namespace ParserOfPsychologists.Application.Components;

public class AccountManager : IAccountManager
{
    private readonly AuthorizationModule _authorizationModule;
    private readonly AccountData _account;

    public AccountManager(AuthorizationModule authorizationModule, AccountData accountData)
    {
        _authorizationModule = authorizationModule;
        _account = accountData;
    }

    public AccountData CurrentAccount { get => _account; }

    public async Task<bool> ConnectAnAccountAsync()
    {
        await LoadAccountProfileAsync();
        await _authorizationModule.SignInAsync(CurrentAccount);

        return true;
    }

    public Task<bool> LoadAccountProfileAsync()
    {
        /*
         *  TODO: Добавить загрузку профиля.
         */
        return Task.FromResult(true);
    }

    public Task<bool> SaveAccountProfileAsync()
    {
        /*
         *  TODO: Добавить сохранение профиля.
         */
        throw new NotImplementedException();
    }
}