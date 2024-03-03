using System.Collections.Generic;
using System.Diagnostics.Tracing;

public abstract class BattleActor
{
    public readonly string name;
    public readonly int level = 1;
    public readonly int maxHp = 0;
    protected readonly int atk = 0;
    protected readonly int def = 0;
    protected readonly int spd = 0;
    public int hp = 0;
    public int stack = 0;
    public readonly List<BattleSkill> skills = new();
    public List<int> status = new();
    public bool turnActive = true;

    public BattleActor(string name, int level, int hp, int maxHp, int atk,  int def, int spd, List<BattleSkill> skills)
    {
        this.name = name;
        this.level = level;
        this.hp = hp;
        this.maxHp = maxHp;
        this.atk = atk;
        this.def = def;
        this.spd = spd;
        this.skills = skills;
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