using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterRack : Node
{
	public BattleCharacter this[int i]
	{
		get
		{
			return GetChild<BattleCharacter>(i); 
		}
	}

	public int Count { get { return GetChildCount(); } }

	public void Add(BattleCharacter character)
    {   
		AddChild(character);
	}

	public void Clear()
	{
		foreach (Node node in GetChildren())
		{
			node.Free();
		}
	}

	//Inserts passed character into index position and returns previous character at that index.
	public BattleCharacter SwapOut(BattleCharacter character, int index)
    {
		BattleCharacter toRemove = GetChild<BattleCharacter>(index);
		RemoveChild(toRemove);
		AddChild(character);
		MoveChild(character, index);
		return toRemove;
	}

	public IEnumerable<BattleCharacter> GetAll()
	{
		return GetChildren().Cast<BattleCharacter>();
	}
}
