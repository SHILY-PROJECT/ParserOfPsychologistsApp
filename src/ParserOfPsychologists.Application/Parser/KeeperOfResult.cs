using CsvHelper;

namespace ParserOfPsychologists.Application.Parser;

public class KeeperOfResult
{
    private static readonly string _dir = "results";

    public static void OpenResultsFolder()
    {
        var dir = new DirectoryInfo(_dir);
        if (!dir!.Exists) dir.Create();
        Process.Start("explorer.exe", dir.FullName);
    }

    public static async Task SaveToFileAsync(IEnumerable<UserModel> users)
    {
        var file = GetFileOfResult(".csv");

        using var streamWriter = new StreamWriter(file.FullName, true, Encoding.UTF8);
        using var сsvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture);
        сsvWriter.Context.RegisterClassMap<UserModelClassMap>();

        await сsvWriter.WriteRecordsAsync(users);

        Process.Start(new ProcessStartInfo("explorer.exe", $@"/n, /select, {file.FullName}")
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Normal
        });
    }

    private static FileInfo GetFileOfResult(string extension)
    {
        var file = new FileInfo(Path.Combine(_dir, $"result   {DateTime.Now:yyyy-MM-dd   HH-mm-ss---fffffff}{extension}"));
        if (!file.Directory!.Exists) file.Directory.Create();
        return file;
    }
}