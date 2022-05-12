namespace ParserOfPsychologists.Application.Parser.Components;

public class StateOfProgressEventArgs
{
    public StateOfProgressEventArgs(int numberOfPagesProcessed, int numberOfUsersProcessed)
    {
        NumberOfPagesProcessed = numberOfPagesProcessed;
        NumberOfUsersProcessed = numberOfUsersProcessed;
    }

    public int NumberOfPagesProcessed { get; set; }
    public int NumberOfUsersProcessed { get; set; }
}