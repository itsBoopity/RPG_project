using Godot;
using System;

public partial class DungeonUI : Control
{
	private Label handSizeLabel;
    private Label cardsLeftLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		handSizeLabel = GetNode<Label>("%HandSizeCount");
        cardsLeftLabel = GetNode<Label>("%CardsLeftCount");
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
