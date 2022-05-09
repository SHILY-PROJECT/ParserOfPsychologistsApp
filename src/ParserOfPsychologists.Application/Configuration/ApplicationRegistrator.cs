namespace ParserOfPsychologists.Application.Configuration;

public static class ApplicationRegistrator
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var cfg = HttpClientConfiguration.CreateConfiguration();

        services
            .AddScoped<IStateOfCityModule, StateOfCityModule>()
            .AddScoped<IParser, MainParser>()
            .AddScoped<IParserWebRequestsFacade, ParserWebFacade>()
            .AddScoped<IHttpClientConfiguration, HttpClientConfiguration>(opt => cfg)
            .AddScoped<IAccountManager, AccountManager>()
            .AddScoped<AccountData>()
            .AddScoped<AuthorizationModule>()
            .AddTransient(opt => HttpHelper.CreateHttpClient(cfg));

        return services;
    }
}