namespace BlackjackCLI;

public class InvalidChoiceException : Exception
{
    public InvalidChoiceException() : base("Cannot choose other option than y (yes) or n (no)!\n" +
                                           "Please restart and follow the instructions!") { }
    
    public InvalidChoiceException(string? message) : base(message) { }
}