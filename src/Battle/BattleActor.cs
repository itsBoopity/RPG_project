using System;
using System.Collections.Generic;

public abstract class BattleActor
{
    public string name;
    public int level = 1;
    public int stack = 0;
    public int hp = 0;
    public int maxHp = 0;
    protected int atk = 0;
    protected int def = 0;
    protected int spd = 0;
    public List<BattleSkill> skills = new();
    public bool turnActive = true;
    public int[] status = new int[3];

    protected BattleActor Clone(BattleActor clone)
    {
        clone.name = name;
        clone.turnActive = turnActive;
        clone.level = level;
        clone.stack = stack;
        clone.hp = hp; clone.maxHp = maxHp;
        clone.atk = atk;
        clone.def = def;
        clone.spd = spd;
        clone.skills = skills;

        return clone;
    }

    /// <summary>
    /// Calculates ATK with all modifiers and effects applied.
    /// </summary>
    public int GetAtk()
    {
        // TODO: Apply passives and recalculations
        return atk;
    }

    /// <summary>
    /// Calculates DEF with all modifiers and effects applied.
    /// </summary>
    public int GetDef()
    {
        // TODO: Apply passives and recalculations
        return def;
    }

     /// <summary>
    /// Calculates SPD with all modifiers and effects applied.
    /// </summary>
    public int GetSpd()
    {
        // TODO: Apply passives and recalculations
        return spd;
    }

}