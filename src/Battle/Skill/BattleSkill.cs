using Godot;
using System;

[Serializable]
public abstract class BattleSkill
{
    public string name;
    public SkillType type;
    public int cost;
    public int cooldown;
    public int currentCooldown = 0;
    public bool snap = false;
    public abstract void Execute(BattleFigure user, BattleFigure target);
    public abstract string Description();

    protected int textureX = 0, textureY = 0;
    public Texture GetIcon()
    {
        return GD.Load<Texture>("res://Images/UI/Battle/SkillIcon/" + this.GetType().Name + ".tres");
    }

    //Called when battle ends, resets cooldowns, can be overriden for skills with special parameters
    public void Reset()
    {
        currentCooldown = 0;
    }
}