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

    public void ExtractSpecialtyAndCity(string specialtyAndCity) =>
        (Specialty, City) = specialtyAndCity.Replace("&nbsp;", "").Split('–') is string[] sc && sc.Length >= 2 ? (sc[0], sc[1]) : ("", "");
}