namespace ParserOfPsychologists.Application.Interfaces;

public interface IParser
{
    event EventHandler<StateOfProgressEventArgs>? StateOfProgressChanged;

    Task<IEnumerable<UserData>> ParseUsersByCityAsync();
}