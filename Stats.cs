namespace BlackjackCLI;

public class Stats
{
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public int Total { get; set; }
    private float _winRatio;

    public void DisplayStats()
    {
        _winRatio = float.Round((Wins / (float)Total), 2);
        
        Console.WriteLine($"Total games: {Total}");
        Console.WriteLine($"Wins: {Wins} | Losses: {Losses} | Draws: {Draws}");
        Console.WriteLine($"W/L Ratio: {_winRatio}");
    }
}