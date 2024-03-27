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

	/// <summary>
	/// Swaps character out.
	/// </summary>
	/// <param name="current">The character currently in rack.</param>
	/// <param name="replace">The character to replace.</param>
	public void SwapOut(BattleCharacter current, BattleCharacter replace)
    {
		int index = current.GetIndex();
		Remove(current);
		AddChild(replace);
		MoveChild(replace, index);
	}

	/// <summary>
	/// Removes the character from rack. Note: doesn't free the BattleCharacter node.
	/// </summary>
	/// <param name="character"></param>
	public void Remove(BattleCharacter character)
    {
		RemoveChild(character);
	}

	public IEnumerable<BattleCharacter> GetAll()
	{
		return GetChildren().Cast<BattleCharacter>();
	}
}
