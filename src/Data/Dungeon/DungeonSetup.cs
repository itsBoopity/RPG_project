using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class DungeonSetup: Resource
{
	[Signal]
	public delegate void DeckSizeChangedEventHandler(int newSize);
	[Export]
	public int StartingHandSize { get; set; }
	[Export]
	public DungeonCardData BossCard { get; set; }
	[Export]
	public Godot.Collections.Array<DungeonCardBundle> DeckDefine
	{
		get
		{
			return deckDefine;
		}
		set
		{
			foreach (DungeonCardBundle bundle in value)
			{
				for (int i=0; i<bundle.Count; i++)
				{
					Deck.Add(bundle.Card);
				}
			}
			ShuffleDeck();
		}
	}
	public List<DungeonCardData> Deck { get; } = new();
	public bool BossCardDrawn { get; private set; }  = false;

	private Godot.Collections.Array<DungeonCardBundle> deckDefine;

	public DungeonSetup() {}

	/// <summary>
	/// Returns the next drawn card from the pile. Returns null if pile is empty.
	/// </summary>
	/// <returns></returns>
	public DungeonCardData DrawFromDeck()
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
			DungeonCardData output = Deck.Last();
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
            DungeonCardData endReplaced = Deck[n-1];
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
