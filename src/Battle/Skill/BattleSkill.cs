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
    public abstract void Execute(BattleFigure user, BattleFigure target);
    public abstract string Description();

    protected int textureX = 0, textureY = 0;
    public void SetIcon(Sprite sprite)
    {
        sprite.RegionRect = new Rect2(textureX * 128, textureY * 128, 128, 128);
    }

    //Called when battle ends, resets cooldowns, can be overriden for skills with special parameters
    public void Reset()
    {
        currentCooldown = 0;
    }
}