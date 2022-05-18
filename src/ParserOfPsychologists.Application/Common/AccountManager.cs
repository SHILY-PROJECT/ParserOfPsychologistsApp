namespace ParserOfPsychologists.Application.Common;

public class AccountManager : IAccountManager
{
    private readonly AccountData _account;

    public AccountManager(AccountData accountData)
    {
        _account = accountData;
    }

    public AccountData Account { get => _account; }

    public Task<bool> LoadAccountProfileAsync()
    {
        /*
         *  TODO: Add load profile.
         */
        return Task.FromResult(true);
    }

    public Task<bool> SaveAccountProfileAsync()
    {
        /*
         *  TODO: Add save profile.
         */
        throw new NotImplementedException();
    }
}