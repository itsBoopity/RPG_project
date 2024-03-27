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
    /// For skills whose Element is SkillElement.VARIABLE, the real Element changes depending on the environment.
    /// For the UI to know which element to show to the play in the tooltip, it calls this method.
    /// </summary>
    /// <returns></returns>
    public virtual SkillElement GetDisplayElement(BattleFieldData bF, BattleCharacter user)
    {
        return Element;
    }

    /// <summary>
    /// If skill has unique activation requirements, check for them here.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsUsableAdditional(BattleActor user){ return true; }

    /// <summary>
    /// For skills that use TargettingType.CUSTOMWINDOW, instances and returns the appropriate .tscn
    /// </summary>
    /// <returns>The UI window that the skill uses to control itself.</returns>
    public virtual SkillCustomWindow GetCustomWindow() { return null; }

    public abstract void Execute(BattleFieldData bF, BattleInteractionData bI);
    public abstract int EstimateDamage(BattleActor user, BattleActor target);

    /// <summary>
    /// Returns the description of the skill
    /// </summary>
    /// <param name="bF">Info about the battlefield, necessary for skills that dynamically change based on its data.</param>
    /// /// <param name="owner">The owner of the skill, necessary for skills that dynamically change based on owner's data.</param>
    public abstract string Description(BattleFieldData bF, BattleCharacter user);
}