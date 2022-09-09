namespace ParserOfPsychologists.Application.Parser;

public class ParserSettings : IParserSettings
{
    private string _cityOnInput = string.Empty;
    private int _pageFrom;
    private int _pageTo;

    public event EventHandler? SettingsChanged;

    public string MainUrl { get => "https://www.b17.ru"; }

    public string CityOnInput
    {
        get => _cityOnInput;
        set { _cityOnInput = value; OnSettingsChanged(); }
    }

    public int PageFrom
    {
        get => _pageFrom;
        set { _pageFrom = value; OnSettingsChanged(); }
    }

    public int PageTo
    {
        get => _pageTo;
        set { _pageTo = value; OnSettingsChanged(); }
    }

    public int TimeoutAfterRequestToOneNumberMainPageWithUsers { get; set; }
    public int TimeoutAfterRequestToOneUserPage { get; set; }
    public int TimeoutAfterRequestToContactsOfOneUser { get; set; }

    public void SetTimeouts(string maskedText)
    {
        var timeouts = Regex
            .Replace(maskedText, @"(сек.*?|\s)", "").Split('-')
            .Select(x => (int)(double.Parse(x) * 1000))
            .ToArray();

        TimeoutAfterRequestToOneNumberMainPageWithUsers = timeouts[0];
        TimeoutAfterRequestToOneUserPage = timeouts[1];
        TimeoutAfterRequestToContactsOfOneUser = timeouts[2];
    }

    protected void OnSettingsChanged() =>
        SettingsChanged?.Invoke(this, EventArgs.Empty);
}