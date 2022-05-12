namespace ParserOfPsychologists.WinFormsUI.Configuration;

public static class WinFormsUIRegistrator
{
    public static IServiceCollection AddWinFormsUI(this IServiceCollection services)
    {
        services
            .AddApplication()
            .AddSingleton<MainForms>()
            .AddTransient<WaitForm>();

        return services;
    }
}