namespace ParserOfPsychologists.Application.Models;

public class ParserSettings : IParserSettings
{
    public string CityOnInput { get; set; } = string.Empty;
    
    public int ToParsePagesFrom { get; set; }
    public int ToParsePagesTo { get; set; }

    public int TimeoutAfterRequestToOneNumberMainPageWithUsers { get; set; }
    public int TimeoutAfterRequestToOneUserPage { get; set; }
    public int TimeoutAfterRequestToContactsOfOneUser { get; set; }

    public void SetTimeouts(string maskedText)
    {
        var timeouts = Regex.Replace(maskedText, @"(сек.*?|\s)", "").Split('-').Select(x => (int)(double.Parse(x) * 1000)).ToArray();
        TimeoutAfterRequestToOneNumberMainPageWithUsers = timeouts[0];
        TimeoutAfterRequestToOneUserPage = timeouts[1];
        TimeoutAfterRequestToContactsOfOneUser = timeouts[2];
    }
}