using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class DungeonHandUI : Control
{
	[Signal]
    public delegate void CardSelectedEventHandler(int index);

    public int Count { get { return GetChildCount(); } }

	public void UpdateHand(List<DungeonCard> hand)
	{
		int childCount = GetChildCount();
		for (int i=0; i<hand.Count; i++)
		{
			if (i < childCount)
			{
				GetChild<DungeonCardUI>(i).SetCard(hand[i]);
			}
			else
			{
				Add(DungeonCardUI.InstantiateFromData(hand[i]));
			}
		}
	}

	public void Add(DungeonCardUI card)
	{
		AddChild(card);
		// Make sure user can't make  any inputs while
		// a card's "being selected" animation is still playing.
		card.StartedAnimation += (DungeonCardUI who) => DisableControl();
		card.Select += ChildChardSelected;
	}

	public void ChildChardSelected(DungeonCardUI card)
	{
		RestoreControl();
		EmitSignal(SignalName.CardSelected, card.GetIndex());
	}

	public void RestoreControl()
	{
		foreach (DungeonCardUI child in GetChildren().Cast<DungeonCardUI>())
		{
			child.Disabled = false;
		}
	}

	public void DisableControl()
	{
		foreach (DungeonCardUI child in GetChildren().Cast<DungeonCardUI>())
		{
			child.Disabled = true;
		}
	}
}
