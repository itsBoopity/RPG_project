using Godot;
using System.Collections.Generic;

public partial class DungeonUI : Control
{
	private DungeonHandUI hand;
	private Label handSizeLabel;
    private Label cardsLeftLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hand = GetNode<DungeonHandUI>("%Hand");
		handSizeLabel = GetNode<Label>("%HandSizeCount");
        cardsLeftLabel = GetNode<Label>("%CardsLeftCount");
	}

	public void UpdateHand(List<DungeonCard> hand)
	{
		this.hand.UpdateHand(hand);
	}

	public void UpdateHandSize(int newSize)
	{
		handSizeLabel.Text = newSize.ToString();
	}

	public void UpdateCardsLeft(int newSize)
	{
		cardsLeftLabel.Text = newSize.ToString();
	}
}
