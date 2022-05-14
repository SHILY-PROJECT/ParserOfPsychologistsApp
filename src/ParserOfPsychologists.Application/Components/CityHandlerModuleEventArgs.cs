namespace ParserOfPsychologists.Application.Parser.Components;

public class CityHandlerModuleEventArgs : EventArgs
{
    public CityHandlerModuleEventArgs() { }
    public CityHandlerModuleEventArgs(int pagesAvailable) => PagesAvailable = pagesAvailable;

    public int PagesAvailable { get; set; }
}