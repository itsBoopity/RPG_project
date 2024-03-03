// TODO: How to do this without storing all skill instances in the class?
using System;
using System.Collections.Generic;

public static class SkillDatabase
{
	private static readonly Dictionary<SkillId, Type> db = new() {
		{SkillId.BasicAttack, typeof(SkillBasicAttack)},
		{SkillId.FirstStrike, typeof(SkillFirstStrike)},
		{SkillId.PrecisionNeedle, typeof(SkillPrecisionNeedle)},
	};

	public static BattleSkill GetSkillData(SkillId id)
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