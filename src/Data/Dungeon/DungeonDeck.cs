using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public partial class DungeonDeck : Resource
{
	[Signal]
	public delegate void DeckSizeChangedEventHandler(int newSize);

	[JsonProperty]
	public bool BossCardDrawn { get; private set; }  = false;
	[JsonProperty]
	public DungeonCard BossCard { get; private set; }
	
	[JsonProperty]
	public List<DungeonCard> Deck { get; private set; } = new();

	public DungeonDeck() {}

	public DungeonDeck(Godot.Collections.Array<DungeonCardBundle> cardBundles, DungeonCard bossCard)
	{
		BossCard = bossCard;
		LoadFromBundles(cardBundles);
	}

	public void LoadFromBundles(Godot.Collections.Array<DungeonCardBundle> cardBundles)
	{
		Deck.Clear();
		foreach (DungeonCardBundle bundle in cardBundles)
		{
			for (int i=0; i<bundle.Count; i++)
			{
				Deck.Add(bundle.Card);
			}
		}
		ShuffleDeck();
	}

	/// <summary>
	/// Returns the next drawn card from the pile. Returns null if pile is empty.
	/// </summary>
	/// <returns></returns>
	public DungeonCard DrawFromDeck()
	{
        if (Deck.Count == 0)
        {
            if (!BossCardDrawn)
            {
                BossCardDrawn = true;
				EmitSignal(SignalName.DeckSizeChanged, CardsLeft());
                return BossCard;
            }
            else
            {
                return null;
            }
        }
		else
		{
			DungeonCard output = Deck.Last();
        	Deck.RemoveAt(Deck.Count - 1);
			EmitSignal(SignalName.DeckSizeChanged, CardsLeft());
			return output;
		}
	}

	public void ShuffleDeck()
    {
        int n = Deck.Count;
        for (int i = n; i > 1; i--)
        {
            int index = (int)(GD.Randi() % n);
            DungeonCard endReplaced = Deck[n-1];
            Deck[n-1] = Deck[index];
            Deck[index] = endReplaced;          
        }
    }

	public int CardsLeft()
	{
		if (BossCardDrawn)
		{
			return Deck.Count;
		}
		else
		{
			return Deck.Count + 1;
		} 
	}

}
