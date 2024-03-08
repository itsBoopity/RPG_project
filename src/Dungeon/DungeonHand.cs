using Godot;

public partial class DungeonHand : Control
{
    public int Count { get { return GetChildCount(); } }

	public void Add(DungeonCard card)
	{
		AddChild(card);
	}

	public void Clear()
	{
		foreach (Node child in GetChildren())
		{
			child.Free();
		}
	}
}
