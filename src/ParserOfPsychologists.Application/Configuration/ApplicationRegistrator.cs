namespace ParserOfPsychologists.Application.Configuration;

public static class ApplicationRegistrator
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ApplicationRegistrator.AddErrorsForVerifyContent();

        var cfg = HttpClientConfiguration.CreateConfiguration();

        services
            .AddScoped<IApplicationFacade, ApplicationFacade>()
            .AddScoped<ICityHandlerModule, CityHandlerModule>()
            .AddScoped<IParser, MainParser>()
            .AddScoped<IPageNavigator, PageNavigator>()
            .AddScoped<IParserSettings, ParserSettings>()
            .AddScoped<IHttpClientConfiguration, HttpClientConfiguration>(opt => cfg)
            .AddScoped<IKeeperOfResult, KeeperOfResult>()
            .AddScoped<IAccountManager, AccountManager>()
            .AddScoped<AccountData>()
            .AddScoped<AuthorizationModule>()
            .AddTransient(opt => HttpHelper.CreateHttpClient(cfg));

        return services;
    }

    private static void AddErrorsForVerifyContent() => HttpHelper.AddErrorsForVerifyContent(new[]
    {
        "Доступ к сайту b17.ru для вашего IP адреса временно заблокирован"
    });
}