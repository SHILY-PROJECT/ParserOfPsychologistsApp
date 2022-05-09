namespace ParserOfPsychologists.Application.Interfaces;

public interface IParser
{
    Task<IEnumerable<UserData>> ParseUsersByCityAsync();
}