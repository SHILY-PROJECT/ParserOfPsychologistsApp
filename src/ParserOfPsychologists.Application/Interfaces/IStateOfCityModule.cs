﻿namespace ParserOfPsychologists.Application.Interfaces;

public interface IStateOfCityModule
{
    string CityId { get; }
    string CityName { get; }
    string LocationRoute { get; }
    Uri Url { get; }
    Dictionary<string, string> Cities { get; }
    Dictionary<string, string> DefaultCities { get; }

    bool IsChanged(string cityName);
    Task<IEnumerable<string>> FindCityAsync(string cityName);
}