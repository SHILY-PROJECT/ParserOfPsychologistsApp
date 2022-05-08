namespace ParserOfPsychologists.ConsoleUI.Configuration;

public static class ConsoleUIRegistrator
{
    public static IServiceCollection AddConsoleUI(this IServiceCollection services)
    {
        services
            .AddApplication()
            .AddSingleton<Startup>()
            .AddScoped<AccountData>();

        return services;
    }
}