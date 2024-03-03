using System.Collections.Generic;

public static class CharacterBattleDataConvertor
{
	public static BattleCharacter ToBattleCharacter(CharacterData c)
	{
		List<BattleSkill> skills = new();
		foreach (SkillId skillId in c.skillIds)
		{
			skills.Add(SkillDatabase.GetSkillData(skillId));
		}
		return new BattleCharacter(c.who, c.name, c.level, c.health, c.maxHealth, c.attack, c.defense, c.speed, skills);
	}

	// public static CharacterData ToCharacterData(BattleCharacter  battleCharacter)
	// {
	// 	CharacterData output = new();
	// 	return output;
	// }
}