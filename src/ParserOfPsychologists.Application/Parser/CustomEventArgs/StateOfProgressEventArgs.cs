namespace ParserOfPsychologists.Application.Parser.CustomEventArgs;

public class StateOfProgressEventArgs : EventArgs
{
    public int NumberOfPagesProcessed { get; set; }
    public int NumberOfUsersProcessed { get; set; }
}