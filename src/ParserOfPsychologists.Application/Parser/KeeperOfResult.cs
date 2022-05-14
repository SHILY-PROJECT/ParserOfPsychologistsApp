using CsvHelper;

namespace ParserOfPsychologists.Application.Parser;

public class KeeperOfResult : IKeeperOfResult
{
    private static readonly string _dir = "results";

    public IList<UserModel> Users { get; set; } = new List<UserModel>();

    public void OpenResultsFolder()
    {
        var dir = new DirectoryInfo(_dir);
        if (!dir!.Exists) dir.Create();
        Process.Start("explorer.exe", dir.FullName);
    }

    public async Task SaveToFileAsync()
    {
        var file = GetFileOfResult(".csv");

        using var streamWriter = new StreamWriter(file.FullName, true, Encoding.UTF8);
        using var сsvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture);
        сsvWriter.Context.RegisterClassMap<UserModelClassMap>();

        await сsvWriter.WriteRecordsAsync(Users);

        Process.Start(new ProcessStartInfo("explorer.exe", $@"/n, /select, {file.FullName}")
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Normal
        });
    }

    private FileInfo GetFileOfResult(string extension)
    {
        var file = new FileInfo(Path.Combine(_dir, $"result   {DateTime.Now:yyyy-MM-dd   HH-mm-ss---fffffff}{extension}"));
        if (!file.Directory!.Exists) file.Directory.Create();
        return file;
    }
}