namespace ParserOfPsychologists.Application.Interfaces;

public interface IKeeperOfResult
{
    IList<UserModel> Users { get; set; }
    void OpenResultsFolder();
    Task SaveToFileAsync();
}