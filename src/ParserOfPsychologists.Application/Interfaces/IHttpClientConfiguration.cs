namespace ParserOfPsychologists.Application.Interfaces;

public interface IHttpClientConfiguration
{
    HttpClientHandler HttpClientHandler { get; }
    CookieContainer CookieContainer { get; }
    Dictionary<string, string> DefaultHeaders { get; }
}