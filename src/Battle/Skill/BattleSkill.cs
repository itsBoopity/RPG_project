using Godot;
using System;

[Serializable]
public abstract class BattleSkill
{
    public string name;
    public SkillType type;
    public TargettingType targetting = TargettingType.NONE;
    public SkillElement element = SkillElement.NONE;
    public bool isAoE = false;
    public int cost;
    public int cooldown;
    public int currentCooldown = 0;
    public bool snap = false;

    private string animationPath = "res://Objects/VFX/Hit.tscn";
    
    // Encapsulates Execute. Actual skill effects are implemented in Execute.
    public bool Use(BattleEngine battleEngine, BattleFigure user, BattleFigure target, float targetEfficiency = 1f)
    {
        if (IsUsable(user) == 0)
        {
            user.stack -= cost;
            currentCooldown = cooldown;
            Execute(battleEngine, user, target, targetEfficiency);
            return true;
        }
        return false;
    }
    // <summary>
    // 0, not usable. 1 not usable because in cooldown, 2 not usable because not enough stacks, 3 user is not active
    // </summary>
    public int IsUsable(BattleFigure user)
    {
        if (currentCooldown > 0)
            return 1;
        else if (user.stack < cost)
            return 2;
        else if (!user.turnActive)
            return 3;
        return 0;
    }
    protected abstract void Execute(BattleEngine battleEngine, BattleFigure user, BattleFigure target, float targetEfficiency);
    public abstract int EstimateDamage(BattleEngine battleEngine, BattleFigure user, BattleFigure target);
    public abstract string Description();
    public Texture GetIcon()
    {
        return GD.Load<Texture>("res://Images/UI/Battle/SkillIcon/" + this.GetType().Name + ".tres");
    }

    public Node2D GetAnimation()
    {
        return GD.Load<PackedScene>(animationPath).Instance<Node2D>();
    }

    //Called when battle ends, resets cooldowns, can be overriden for skills with special parameters
    public virtual void Reset()
    {
        currentCooldown = 0;
    }
}