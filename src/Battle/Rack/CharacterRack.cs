using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CharacterRack : Node
{
	[Signal]
	public delegate void CharacterRackChangedEventHandler();

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
	/// Replaces current character with a different one. Replacement must not already be in rack, otherwise use Swap().
	/// </summary>
	/// <param name="current">The character currently in rack.</param>
	/// <param name="replace">The character to replace.</param>
	public void Replace(BattleCharacter current, BattleCharacter replace)
    {
		if (ContainsBattleCharacter(replace))
		{
			throw new ArgumentException("CharacterRack::Replace() trying to add character that is already in this rack.");
		}
		else if (!ContainsBattleCharacter(current))
		{
			throw new ArgumentException("CharacterRack::Replace() trying to replace character that isn't currently in the rack.");
		}
		int index = current.GetIndex();
		Remove(current);
		AddChild(replace);
		MoveChild(replace, index);
		EmitSignal(SignalName.CharacterRackChanged);
	}

	/// <summary>
	/// Swaps two BattleCharacter's positions within the CharacterRack. Must contain both BattleCharacters.
	/// </summary>
	/// <param name="current">One of the characters to be swapped.</param>
	/// <param name="replace">One of the characters to be swapped.</param>
	public void Swap(BattleCharacter first, BattleCharacter second)
	{
		if (!ContainsBattleCharacter(first) || !ContainsBattleCharacter(second))
		{
			throw new ArgumentException("CharacterRack::Swap() Trying to swap character(s) that are not currently in the rack.");
		}
		var temp = first.GetIndex();
		MoveChild(first, second.GetIndex());
		MoveChild(second, temp);
		EmitSignal(SignalName.CharacterRackChanged);
	}

	/// <summary>
	/// Checks if CharacterRack contains character.
	/// </summary>
	public bool ContainsBattleCharacter(BattleCharacter character)
	{
		return character.GetParent() == this;
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

	public List<BattleCharacter> GetAllAsCharacterList()
	{
		List<BattleCharacter> output = new();
		foreach (BattleCharacter character in GetChildren().Cast<BattleCharacter>())
		{
			output.Add(character);
		}
		return output;
	}

	public List<BattleActor> GetAllAsActorList()
	{
		List<BattleActor> output = new();
		foreach (BattleCharacter character in GetChildren().Cast<BattleCharacter>())
		{
			output.Add(character);
		}
		return output;
	}
}
