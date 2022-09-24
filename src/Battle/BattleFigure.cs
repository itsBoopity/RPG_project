using System;
using System.Collections.Generic;

[Serializable]
public abstract class BattleFigure
{
    public bool turnActive = true;

    public int Lvl = 1;
    public int stack = 0;
    public int HP = 0, maxHP = 0;
    public int ATK = 0;
    public int DEF = 0;
    public int SPD = 0;
    
    // If you're adding new stats, don't forget to add them to the cloning method!!!
    public List<BattleSkill> skills = new List<BattleSkill>();
    public int[] status = new int[3];

    protected BattleFigure Clone(BattleFigure clone)
    {
        clone.turnActive = turnActive;
        clone.Lvl = Lvl;
        clone.stack = stack;
        clone.HP = HP; clone.maxHP = maxHP;
        clone.ATK = ATK;
        clone.DEF = DEF;
        clone.SPD = SPD;
        clone.skills = skills;

        return clone;
    }

    public int GetATK()
    {
        // Apply passives and recalculations
        return ATK;
    }
    public int GetDEF()
    {
        // Apply passives and recalculations
        return DEF;
    }

}