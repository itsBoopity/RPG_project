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
            hp = 11,
            maxHp = 11,
            atk = 3,
            def = 1,
            spd = 8,
			skillIds = new List<string>{"s_firststrike", "s_precisionneedle"}
        };
	}
}