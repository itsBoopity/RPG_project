using System.Threading;
using Godot;

public abstract partial class BattleSkillData: Resource
{
    private string name;
    public SkillId Id { get; protected set; }
    public string DisplayName { get{ return Tr(name);} }
    public SkillType Type { get; protected set; }
    public SkillElement Element { get; protected set; }
    public TargettingType Targetting { get; protected set; }
    public bool IsAoE { get; protected set; } = false;
    public int Cost { get; protected set; }
    public int Cooldown { get; protected set; }
    public bool Snap { get; protected set; } = false;

    private string animationPath = "res://Objects/VFX/Hit.tscn";
    public BattleSkillData( SkillId id, string name, SkillType skillType, SkillElement element,
                        TargettingType targetting, bool isAoE, int cost, int cooldown, bool snap)
    { 
        this.name = name;
        Id = id; 
        Type = skillType;
        Element = element;
        Targetting = targetting;
        IsAoE = isAoE;
        Cost = cost;
        Cooldown = cooldown;
        Snap = snap;
    }
    public Texture2D GetIcon()
    {
        return GD.Load<Texture2D>($"res://Images/UI/Battle/SkillIcon/{Id}.tres");
    }

    public AnimatedSpriteOneOff GetAnimation()
    {
        return GD.Load<PackedScene>(animationPath).Instantiate<AnimatedSpriteOneOff>();
    }

    
    /// <summary>
    /// Called at the start of every battle. Used by specific skill implementations that need to set up data. 
    /// </summary>
    public virtual void Initialize() {}

    /// <summary>
    /// If skill has unique activation requirements, check for them here.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsUsableAdditional(BattleActor user)
    {
        return true;
    }

    public abstract void Execute(BattleFieldData bf, BattleInteractionData bInteraction);
    public abstract int EstimateDamage(BattleActor user, BattleActor target);
    public abstract string Description();
}