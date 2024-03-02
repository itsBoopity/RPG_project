using System.Collections.Generic;

public static class CharacterBattleDataConvertor
{
	public static BattleCharacter ToBattleCharacter(CharacterData c)
	{
		List<BattleSkill> skills = new();
		foreach (string skillId in c.skillIds)
		{
			skills.Add(SkillDatabase.GetSkillData(skillId));
		}
		return new BattleCharacter(c.name, c.who, c.hp, c.maxHp, c.atk, c.def, c.spd, skills);
	}

	// public static CharacterData ToCharacterData(BattleCharacter  battleCharacter)
	// {
	// 	CharacterData output = new();
	// 	return output;
	// }
}