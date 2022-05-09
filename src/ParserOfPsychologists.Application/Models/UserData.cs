namespace ParserOfPsychologists.Application.Models;

public record UserData
{
    public UserData() { }
    public UserData(string fullName) => FullName = fullName;
    public UserData(string fullName, Uri userUrl) => (FullName, UrlOnSite) = (fullName, userUrl.OriginalString);

    public string FullName { get; set; } = string.Empty;
    public string UrlOnSite { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public UserContactsData? Contacts { get; set; }

    public void ExtractSpecialtyAndCity(string specialtyAndCity)
    {
        var sc = specialtyAndCity.Replace("&nbsp;", "").Split('–');
        Specialty = sc.FirstOrDefault() ?? string.Empty;
        City = sc.LastOrDefault() ?? string.Empty;
    }
}