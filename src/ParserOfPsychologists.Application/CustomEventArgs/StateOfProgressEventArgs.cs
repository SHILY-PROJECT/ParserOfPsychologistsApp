namespace ParserOfPsychologists.Application.CustomEventArgs;

public class StateOfProgressEventArgs : EventArgs
{
    public int NumberOfPagesProcessed { get; set; }
    public int NumberOfUsersProcessed { get; set; }
}