namespace ParserOfPsychologists.Application.Parser;

public class PageNavigator
{
    private readonly ICityHandlerModule _city;
    private readonly IParserSettings _settings;
    public int _currentPageNumber;

    public PageNavigator(IParserSettings settings, ICityHandlerModule city)
    {
        _settings = settings;
        _city = city;
        this.SetData();
    }

    public Uri CurrentPage { get => new($"{_city.CityUrl.OriginalString}?page={_currentPageNumber}"); }
    public Uri PrevPage { get; private set; } = null!;

    public bool MoveNextOnPage()
    {
        if (_currentPageNumber >= _settings.NumberOfPagesToParse) return false;

        if (_currentPageNumber > 0)
            PrevPage = CurrentPage;
        _currentPageNumber++;

        return true;
    }

    public void Reset() => this.SetData();

    private void SetData()
    {
        _currentPageNumber = 0;
        PrevPage = new(_city.CityUrl.OriginalString);
    }
}