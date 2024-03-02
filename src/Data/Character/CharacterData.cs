using System;
using System.Collections.Generic;

[Serializable]
public class CharacterData
{
    public CharacterEnum who;
    public string name;
    public int level;
    public int hp;
    public int maxHp;
    public int atk;
    public int def;
    public int spd;
    public List<string> skillIds;

	public void UpdateData(BattleCharacter character)
	{
		if (character.who != who)
		{
			throw new ArgumentException($"CharacterData::UpdateData(BattleCharacter) expected {who} enum, but got {character.who}");
		}
		hp = character.hp;
	}
}