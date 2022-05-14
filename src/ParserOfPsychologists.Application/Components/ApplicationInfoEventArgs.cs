namespace ParserOfPsychologists.Application.Parser.Components;

public class ApplicationInfoEventArgs : EventArgs
{
    public ApplicationInfoEventArgs() { }
    public ApplicationInfoEventArgs(string message) => Message = message;

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Exception Ex { get; set; } = null!;
}