namespace BlackjackCLI;

public class Player
{
    public string Name { get; init; } = "Player";
    public int PlayerScore { get; set; }
    public List<Card> PlayerCards { get; set; } = new();

    public int CalculateScore()
    {
        int total = 0;
        int aces = 0;

        foreach (var card in PlayerCards)
        {
            total += (int)card.Rank;
            if (card.Rank == Rank.Ace)
                aces++;
        }
        // Ace 11 -> 1
        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }

        return total;
    }
    
    // Przeciążenie operatorów > i < do porównania score graczy w przypadku,
    // gdy żaden z graczy nie przekroczył 21.
    
    public static bool operator > (Player p1, Player p2)
    {
        return p1.CalculateScore() > p2.CalculateScore();
    }
    public static bool operator < (Player p1, Player p2)
    {
        return p1.CalculateScore() < p2.CalculateScore();
    }
}