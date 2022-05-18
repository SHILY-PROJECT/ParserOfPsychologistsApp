namespace ParserOfPsychologists.Application.Interfaces;

public interface IAuthorization
{
    Task<bool> SignInAsync();
}