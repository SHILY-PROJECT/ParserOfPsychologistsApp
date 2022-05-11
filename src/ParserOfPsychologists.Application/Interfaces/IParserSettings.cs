namespace ParserOfPsychologists.Application.Interfaces;

public interface IParserSettings
{
    string CityOnInput { get; set; }

    int ToParsePagesFrom { get; set; }
    int ToParsePagesTo { get; set; }

    int TimeoutAfterRequestToOneNumberMainPageWithUsers { get; set; }
    int TimeoutAfterRequestToOneUserPage { get; set; }
    int TimeoutAfterRequestToContactsOfOneUser { get; set; }

    void SetTimeouts(string maskedText);
}