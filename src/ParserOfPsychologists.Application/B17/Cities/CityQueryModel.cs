namespace ParserOfPsychologists.Application.B17.Cities;

public record CityQueryModel
{
    public CityQueryModel(string name, string param) => (Name, Param) = (name, param);

    public string Name { get; set; } = string.Empty;
    public string Param { get; set; } = string.Empty;
    public string SearchQueryText { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;

    public int AvailablePages { get; set; } = -1;
    public int StartPageNumber { get; set; }
    public int EndPageNumber { get; set; }
}