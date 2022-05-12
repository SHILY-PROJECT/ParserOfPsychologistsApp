namespace ParserOfPsychologists.Application.Configuration;

public static class ApplicationRegistrator
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var cfg = HttpClientConfiguration.CreateConfiguration();

        services
            .AddScoped<IApplicationFacade, ApplicationFacade>()
            .AddScoped<ICityHandlerModule, CityHandlerModule>()
            .AddScoped<IParser, MainParser>()
            .AddScoped<IPageNavigator, PageNavigator>()
            .AddScoped<IParserSettings, ParserSettings>()
            .AddScoped<IHttpClientConfiguration, HttpClientConfiguration>(opt => cfg)
            .AddScoped<IAccountManager, AccountManager>()
            .AddScoped<AccountData>()
            .AddScoped<AuthorizationModule>()
            .AddTransient(opt => HttpHelper.CreateHttpClient(cfg));

        return services;
    }
}