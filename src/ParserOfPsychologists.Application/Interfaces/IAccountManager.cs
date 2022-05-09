namespace ParserOfPsychologists.Application.Interfaces;

public interface IAccountManager
{
    AccountData CurrentAccount { get; }
    Task ConnectAnAccountAsync();
    Task<bool> SaveAccountProfileAsync();
    Task<bool> LoadAccountProfileAsync();
}