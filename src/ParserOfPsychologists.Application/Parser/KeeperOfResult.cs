namespace ParserOfPsychologists.Application.Parser;

public class KeeperOfResult
{
    public static void SaveToFile(IEnumerable<UserData> users)
    {
        var file = GetFileOfResult(".csv");

    }

    private static FileInfo GetFileOfResult(string extension)
    {
        var file = new FileInfo(Path.Combine("results", $"result   {DateTime.Now:yyyy-MM-dd   HH-mm-ss---fffffff}{extension}"));
        if (!file.Directory!.Exists) file.Directory.Create();
        return file;
    }
}