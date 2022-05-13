namespace ParserOfPsychologists.Application.Interfaces;

public interface IParserSettings
{
    event EventHandler? SettingsChanged;

    string MainUrl { get; }

    int PageFrom { get; set; }
    int PageTo { get; set; }

    int TimeoutAfterRequestToOneNumberMainPageWithUsers { get; set; }
    int TimeoutAfterRequestToOneUserPage { get; set; }
    int TimeoutAfterRequestToContactsOfOneUser { get; set; }

    string CityOnInput { get; set; }

    void SetTimeouts(string maskedText);
}