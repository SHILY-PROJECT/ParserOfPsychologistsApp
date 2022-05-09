namespace ParserOfPsychologists.Application.Parser.Components;

public class StateOfCityEventArgs : EventArgs
{
    public StateOfCityEventArgs() { }
    public StateOfCityEventArgs(int pagesAvailable) => PagesAvailable = pagesAvailable;

    public int PagesAvailable { get; set; }
}