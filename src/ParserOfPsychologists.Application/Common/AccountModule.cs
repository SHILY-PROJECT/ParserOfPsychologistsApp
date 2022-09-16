using System.Net;

namespace ParserOfPsychologists.Application.Models;

public class AccountModule
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public CookieContainer CookieContainer { get; } = new();
}