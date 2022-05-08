namespace ParserOfPsychologists.ConsoleUI;

internal class Startup
{
    private readonly IParserWebRequestsFacade _webRequestsFacade;

    public Startup(IParserWebRequestsFacade webRequestsFacade)
    {
        _webRequestsFacade = webRequestsFacade;
    }
    
    public async Task Run()
    {
        var resp = await _webRequestsFacade.ParseUsersByCityAsync(new Uri("https://www.b17.ru/psiholog/moskva/"));
        Console.WriteLine();
        Console.ReadKey();
    }

    internal static async Task Main(string[] args) =>
        await CreateHostBuilder(args).Build().Services.GetRequiredService<Startup>().Run();

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices(services => services.AddConsoleUI());
}