using CsvHelper;
using System.Text;
using System.Diagnostics;
using ParserOfPsychologists.Application.Models;
using ParserOfPsychologists.Application.Interfaces;
using ParserOfPsychologists.Application.Configuration;

namespace ParserOfPsychologists.Application.Common;

public class KeeperOfResult : IKeeperOfResult
{
    private static DirectoryInfo Dir
    {
        get
        {
            var dir = new DirectoryInfo("results");
            if (!dir.Exists) dir.Create();
            return dir;
        }
    }

    public IList<UserModel> Users { get; set; } = new List<UserModel>();

    public void OpenResultsFolder() =>
        Process.Start("explorer.exe", Dir.FullName);

    public async Task SaveToFileAsync()
    {
        if (!Users.Any()) return;

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

    private FileInfo GetFileOfResult(string extension) =>
        new(Path.Combine(Dir.FullName, $"result   {DateTime.Now:yyyy-MM-dd   HH-mm-ss---fffffff}{extension}"));
}