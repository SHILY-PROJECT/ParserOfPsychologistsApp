namespace ParserOfPsychologists.Application.Toolkit;

public static class UserExtensions
{
    public static void ExtractSpecialtyAndCity(this UserModel user, string specialtyAndCity) =>
        (user.Specialty, user.City) = specialtyAndCity.Replace("&nbsp;", "").Split('–') is string[] sc && sc.Length >= 2 ? (sc[0], sc[1]) : ("", "");
}