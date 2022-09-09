namespace ParserOfPsychologists.Application.Interfaces;

public interface IAccountManager
{
    AccountData Account { get; }
    Task<bool> SaveAccountProfileAsync();
    Task<bool> LoadAccountProfileAsync();
}