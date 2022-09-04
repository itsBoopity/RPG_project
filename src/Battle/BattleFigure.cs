using System;
using System.Collections.Generic;

[Serializable]
public abstract class BattleFigure
{
    public int Lvl = 1;
    public int stack = 0;
    public int HP = 0, maxHP = 0;
    public int ATK = 0;
    public int DEF = 0;
    public List<BattleSkill> skills = new List<BattleSkill>();
    public int[] status = new int[3];

    protected BattleFigure Clone(BattleFigure clone)
    {
        clone.HP = HP; clone.maxHP = maxHP;
        clone.ATK = ATK;
        clone.DEF = DEF;
        clone.skills = skills;

        return clone;
    }

}