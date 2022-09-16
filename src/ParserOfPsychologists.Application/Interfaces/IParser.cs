using ParserOfPsychologists.Application.Models;
using ParserOfPsychologists.Application.CustomEventArgs;

namespace ParserOfPsychologists.Application.Interfaces;

public interface IParser
{
    event EventHandler<StateOfProgressEventArgs>? StateOfProgressChanged;

    IEnumerable<UserModel> ParseUsers();
    Task<IEnumerable<UserModel>> ParseUsersAsync();
}