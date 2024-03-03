using System;
using System.Collections.Generic;

/// <summary>
/// Creates starting CharacterData for any Character.
/// </summary>
public static class CharacterDataConstructor
{
	public static CharacterData Create(CharacterEnum who)
	{
        return who switch
        {
            CharacterEnum.CLAUS => CreateClaus(),
            _ => throw new ArgumentException($"CharacterDataConstructor::Create doesn't recognize enum {who}"),
        };
    }

	private static CharacterData CreateClaus()
	{
        return new CharacterData()
        {
            name = "Claus",
            who = CharacterEnum.CLAUS,
            health = 11,
            maxHealth = 11,
            attack = 3,
            defense = 1,
            speed = 8,
			skillIds = new List<SkillId> { SkillId.FirstStrike, SkillId.PrecisionNeedle }
        };
	}
}