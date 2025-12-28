using BlackjackCLI;

Console.WriteLine("Welcome to console Blackjack!");
Console.WriteLine("Would you like to play a round? ( y / n )");

try
{
    var input = Console.ReadLine();
    if (input == "y")
    {
        using (var game = new Game())
        {
            game.Play();
        }
    }
    else if (input == "n")
    {
        Console.WriteLine("Goodbye");
    }
    
    if (input != "y" & input != "n")
    {
        throw new InvalidChoiceException();     // Zastosowanie customowego wyjątku
    }
}
catch (InvalidChoiceException e)
{
    Console.WriteLine(e.Message);
}

public enum Color
{
    Hearts,
    Diamonds,
    Spades,
    Clubs
};

public enum Rank
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 10,
    Queen = 10,
    King = 10,
    Ace = 11 // domyślnie 11
};

public delegate void CardDrawHandler(Player player, Card card);
public delegate void RoundEndHandler(Player player, Player dealer, string result);

public class Game : IDisposable
{
    private bool _disposed = false;
    
    public event CardDrawHandler? OnCardDraw;
    public event RoundEndHandler? OnRoundEnd;
    
    private void DrawCardFor(Player player, Deck deck)
    {
        var card = deck.DrawCard();
        player.PlayerCards.Add(card);
        OnCardDraw?.Invoke(player, card);
    }
    
    public void Play()
    {
        OnCardDraw += (player, card) =>
        {
            Console.WriteLine($"{player.Name} draws: {card}");
        };
        OnRoundEnd += (player, dealer, result) =>
        {
            Console.WriteLine($"Round ended. Player score: {player.CalculateScore()}, " +
                              $"Dealer score: {dealer.CalculateScore()}. Result: {result}");
        };
        
        while (true)
        {
            var player = new Player();
            var dealer = new Player()
            {
                Name = "Dealer"
            };
            var deck = new Deck();
            deck.Shuffle();
            
            DrawCardFor(player, deck);
            DrawCardFor(player, deck);
            DrawCardFor(dealer, deck);
            DrawCardFor(dealer, deck);
            
            Console.Clear();
            Console.WriteLine($"{dealer.Name} shows: {dealer.PlayerCards[0]}");
            Console.WriteLine($"\n{player.Name} has:");
            foreach (var card in player.PlayerCards)
            {
                Console.WriteLine(card);
            }
            Console.WriteLine($"Your score: {player.CalculateScore()}");

            while (true)
            {
                Console.WriteLine("\nHit or Stand? ( h / s )");
                try
                {
                    var choice = Console.ReadLine();
                    if (choice == "h")
                    {
                        var newCard = deck.DrawCard();
                        player.PlayerCards.Add(newCard);
                        Console.WriteLine("You drew " + newCard);
                        Console.WriteLine("Your score: " + player.CalculateScore());

                        if (player.CalculateScore() > 21)
                        {
                            Console.WriteLine("You busted!");
                            break;
                        }
                    }
                    else if (choice == "s")
                    {
                        break;
                    }

                    if (choice != "h" & choice != "s")
                    {
                        throw new InvalidChoiceException("Cannot choose other option than h (hit) or s (stand)!");  // Zastosowanie customowego wyjątku
                    }
                }
                catch (InvalidChoiceException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
            
            if (player.CalculateScore() <= 21)
            {
                Console.WriteLine("\nDealer's turn:");

                foreach (var c in dealer.PlayerCards)
                    Console.WriteLine(c);

                while (dealer.CalculateScore() < 17)
                {
                    var card = deck.DrawCard();
                    dealer.PlayerCards.Add(card);
                    Console.WriteLine("Dealer draws " + card);
                }

                Console.WriteLine("Dealer score: " + dealer.CalculateScore());
            }
            player.PlayerScore = player.CalculateScore();
            dealer.PlayerScore = dealer.CalculateScore();

            string result;

            if (player.PlayerScore > 21)
                result = "Dealer wins!";
            else if (dealer.PlayerScore > 21)
                result = "You win!";
            else if (player > dealer)
                result = "You win!";
            else if (dealer > player)
                result = "Dealer wins!";
            else
                result = "It's a draw!";
            
            Console.WriteLine("\n ---RESULT---");
            OnRoundEnd?.Invoke(player, dealer, result);

            Console.WriteLine("\nPlay again? (y/n)");
            try
            {
                var again = Console.ReadLine();
                if (again == "n")
                {
                    Console.WriteLine("Goodbye!");
                    return;
                }

                if (again != "y" & again != "n")
                {
                    throw new InvalidChoiceException("Cannot choose other option than y (yes) or n (no)!");  // Zastosowanie customowego wyjątku
                }
            }
            catch (InvalidChoiceException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            OnCardDraw = null;
            OnRoundEnd = null;
            Console.WriteLine("Game resources disposed.");
            _disposed = true;
        }
    }
}