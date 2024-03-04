using Godot;

public abstract class BattleSkill
{
    public readonly SkillId id;
    public readonly string name;
    public readonly SkillType type;
    public readonly SkillElement element = SkillElement.NONE;
    public readonly TargettingType targetting = TargettingType.NONE;
    public readonly bool isAoE = false;
    public readonly int cost;
    public readonly int cooldown;
    public readonly bool snap = false;
    public int currentCooldown = 0;

    private string animationPath = "res://Objects/VFX/Hit.tscn";

    public BattleSkill( SkillId id, string name, SkillType skillType, SkillElement element,
                        TargettingType targetting, bool isAoE, int cost, int cooldown, bool snap)
    { 
        this.id = id; 
        this.name = name;
        this.type = skillType;
        this.element = element;
        this.targetting = targetting;
        this.isAoE = isAoE;
        this.cost = cost;
        this.cooldown = cooldown;
        this.snap = snap;
    }
    
    // Encapsulates Execute. Actual skill effects are implemented in Execute.
    public bool Use(BattleEngine battleEngine, IBattleActor user, IBattleActor target, float appendageCoef = 1f)
    {
        if (IsUsable(user) == 0)
        {
            user.ChangeStack(-cost);
            currentCooldown = cooldown;
            Execute(battleEngine, user, target, appendageCoef);
            return true;
        }
        return false;
    }
    // <summary>
    // 0, not usable. 1 not usable because in cooldown, 2 not usable because not enough stacks, 3 user is not active
    // </summary>
    public int IsUsable(IBattleActor user)
    {
        if (currentCooldown > 0)
            return 1;
        else if (user.Stack < cost)
            return 2;
        else if (user is BattleCharacter character && !character.TurnActive)
            return 3;
        return 0;
    }
    protected abstract void Execute(BattleEngine battleEngine, IBattleActor user, IBattleActor target, float appendageCoef);
    public abstract int EstimateDamage(BattleEngine battleEngine, IBattleActor user, IBattleActor target);
    public abstract string Description();
    public Texture2D GetIcon()
    {
        return GD.Load<Texture2D>($"res://Images/UI/Battle/SkillIcon/{id}.tres");
    }

    public Node2D GetAnimation()
    {
        return GD.Load<PackedScene>(animationPath).Instantiate<Node2D>();
    }

    //Called when battle ends, resets cooldowns, can be overriden for skills with special parameters
    public virtual void Reset()
    {
        currentCooldown = 0;
    }
}