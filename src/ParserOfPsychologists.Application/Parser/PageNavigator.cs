﻿namespace ParserOfPsychologists.Application.Parser;

public class PageNavigator
{
    private readonly IParserSettings _parserSettings;
    private readonly ICityHandlerModule _cityHandler;

    public int _currentPageNumber;

    public PageNavigator(IParserSettings parserSettings, ICityHandlerModule cityHandler)
    {
        _parserSettings = parserSettings;
        _cityHandler = cityHandler;
        _parserSettings.SettingsChanged += OnConfigure;
    }

    public Uri CurrentPage { get => new($"{_cityHandler.CityUrl.OriginalString}?page={_currentPageNumber}"); }
    public Uri PrevPage { get; private set; } = null!;

    public bool MoveNextOnPage()
    {
        if (_currentPageNumber > _parserSettings.PageTo) return false;

        if (_currentPageNumber > _parserSettings.PageFrom)
            PrevPage = CurrentPage;
        _currentPageNumber++;

        return true;
    }

    private void OnConfigure(object? source, EventArgs args)
    {
        if (_currentPageNumber == _parserSettings.PageFrom &&
            PrevPage != null && _cityHandler.CityUrl.OriginalString == PrevPage.OriginalString) return;

        _currentPageNumber = _parserSettings.PageFrom;
        PrevPage = new(_cityHandler.CityUrl.OriginalString);
    }
}