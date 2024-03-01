using System.Collections.Generic;

public struct CharacterData
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
}