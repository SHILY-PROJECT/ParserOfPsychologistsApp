namespace ParserOfPsychologists.Application.Interfaces;

public interface IParserWebRequestsFacade
{
    Task<bool> ConnectAnAccountAsync();
    Task<IReadOnlyCollection<string>> FindCityAsync(string cityName);
    Task<IReadOnlyCollection<UserData>> ParseUsersByCityAsync();
}