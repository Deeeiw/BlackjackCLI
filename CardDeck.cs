namespace BlackjackCLI;

public class Card
{
    public Color Color { get; }
    public Rank Rank { get; }

    public Card(Color color, Rank rank)
    {
        Color = color;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank} of {Color}";
    }
};

public class Deck
{
    private List<Card> _cards;
    private Random _rng = new Random();

    public Deck()
    {
        _cards = new List<Card>();
        foreach (Color color in Enum.GetValues(typeof(Color)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                _cards.Add(new Card(color, rank));
            }
        }
    }

    public void Shuffle()
    {
        for (int i = _cards.Count - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }

    public Card DrawCard()
    {
        Card card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
};