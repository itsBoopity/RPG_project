using System.Collections.Generic;

/// <summary>
/// A class representing a player character during battles.
/// </summary>
public class BattleCharacter: BattleActor
{
    public readonly CharacterEnum who;
    public BattleCharacter( CharacterEnum who,string name, int level, int hp, int maxHp,
                            int atk, int def, int spd, List<BattleSkill> skills)
        : base(name, level, hp, maxHp, atk, def, spd, skills)
    {
        this.who = who;
    }

}