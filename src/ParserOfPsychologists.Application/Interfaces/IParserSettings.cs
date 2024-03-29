﻿namespace ParserOfPsychologists.Application.Interfaces;

public interface IParserSettings
{
    event EventHandler? SettingsChanged;

    int PageFrom { get; set; }
    int PageTo { get; set; }
    int TimeoutAfterRequestToOneNumberMainPageWithUsers { get; set; }
    int TimeoutAfterRequestToOneUserPage { get; set; }
    int TimeoutAfterRequestToContactsOfOneUser { get; set; }
    string MainUrl { get; }
    string CityOnInput { get; set; }

    void SetTimeouts(string maskedText);
}