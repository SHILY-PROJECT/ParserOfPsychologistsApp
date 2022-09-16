namespace ParserOfPsychologists.WinFormsUI.Configuration;

public static class WinFormsUIRegistration
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