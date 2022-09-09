namespace ParserOfPsychologists.WinFormsUI;

internal static class Startup
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        System.Windows.Forms.Application.Run(CreateHostBuilder().Build().Services.GetRequiredService<MainForms>());
    }

    private static IHostBuilder CreateHostBuilder() => Host
        .CreateDefaultBuilder(Array.Empty<string>())
        .ConfigureServices(services => services.AddWinFormsUI());
}