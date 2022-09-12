using System;
using System.Collections.Generic;

[Serializable]
public abstract class BattleFigure: Godot.Object
{
    public int Lvl = 1;
    public int stack = 0;
    public int HP = 0, maxHP = 0;
    public int ATK = 0;
    public int DEF = 0;
    public int SPD = 0;

    ~BattleFigure() { Godot.GD.Print("> Freed Figure"); this.Free(); }
    
    // If you're adding new stats, don't forget to add them to the cloning method!!!
    public List<BattleSkill> skills = new List<BattleSkill>();
    public int[] status = new int[3];

    protected BattleFigure Clone(BattleFigure clone)
    {
        clone.Lvl = Lvl;
        clone.stack = stack;
        clone.HP = HP; clone.maxHP = maxHP;
        clone.ATK = ATK;
        clone.DEF = DEF;
        clone.SPD = SPD;
        clone.skills = skills;

        return clone;
    }

}