namespace ParserOfPsychologists.WinFormsUI.Configuration;

public static class WinFormsUIRegistrator
{
    public static IServiceCollection AddWinFormsUI(this IServiceCollection services)
    {
        services
            .AddSingleton<MainForms>()
            .AddApplication();

        return services;
    }
}