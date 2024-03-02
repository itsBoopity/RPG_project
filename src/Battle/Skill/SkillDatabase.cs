// TODO: How to do this without storing all skill instances in the class?
using System;
using System.Collections.Generic;

public static class SkillDatabase
{
	private static readonly Dictionary<string, Type> db = new Dictionary<string, Type>{
		{"s_basicattack", typeof(SkillBasicAttack)},
		{"s_firststrike", typeof(SkillFirstStrike)},
		{"s_precisionneedle", typeof(SkillPrecisionNeedle)},
		
	};

	public static BattleSkill GetSkillData(string id)
	{
		if (!db.ContainsKey(id))
		{
			throw new ArgumentException($"SkillDatabase::GetSkillData does not recognize skill id '{id}'");
		}
		else
		{
			return (BattleSkill)Activator.CreateInstance(db[id]);
		}
	}
}