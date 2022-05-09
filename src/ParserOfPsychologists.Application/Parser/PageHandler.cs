namespace ParserOfPsychologists.Application.Parser;

public class PageHandler
{
    private readonly IStateOfCityModule _city;
    private readonly IParserSettings _settings;
    public int _currentPageNumber;

    public PageHandler(IParserSettings settings, IStateOfCityModule city)
    {
        _settings = settings;
        _city = city;
        this.SetData();
    }

    public Uri CurrentPage { get => new($"{_city.Url.OriginalString}?page={_currentPageNumber}"); }
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
        PrevPage = new(_city.Url.OriginalString);
    }
}