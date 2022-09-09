namespace ParserOfPsychologists.Application.Interfaces;

public interface IPageNavigator
{
    Uri CurrentPage { get; }
    Uri PrevPage { get; }
    bool MoveNextOnPage();
}