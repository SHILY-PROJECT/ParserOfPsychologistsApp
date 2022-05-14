namespace ParserOfPsychologists.Application.Parser.Components;

public class StateOfProgressEventArgs : EventArgs
{
    public int NumberOfPagesProcessed { get; set; }
    public int NumberOfUsersProcessed { get; set; }
}