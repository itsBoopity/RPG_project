using System;
using System.Collections.Generic;


/// <summary>
/// A class representing general player character data. 
/// </summary>
[Serializable]
public class CharacterData
{
    public CharacterEnum who;
    public string name;
    public int level;
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public int speed;
    public List<SkillId> skillIds;

	public void UpdateData(BattleCharacter character)
	{
		if (character.Who != who)
		{
			throw new ArgumentException($"CharacterData::UpdateData(BattleCharacter) expected {who} enum, but got {character.Who}");
		}
		health = character.Health;
	}
}