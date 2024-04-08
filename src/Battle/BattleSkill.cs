using Godot;

public class BattleSkill
{
	private readonly BattleSkillData data;

	public int CurrentCooldown { get; set; } = 0;

    public TargettingType Targetting { get { return data.Targetting; } }
    public string DisplayName { get { return data.DisplayName; } }
    public int Cooldown { get { return data.Cooldown; } }
    public int Cost { get { return data.Cost; } }
    public SkillType Type { get { return data.Type; } }
    public SkillElement Element { get { return data.Element; } }
    public bool IsAoE  { get { return data.IsAoE; } }
    public bool Snap { get { return data.Snap; } }
    public Texture2D Icon { get { return data.GetIcon(); } }
    public AnimatedSpriteOneOff Animation { get { return data.GetAnimation(); } }
    
    public BattleSkill() {}

	public BattleSkill(BattleSkillData skill)
	{
		data = skill;
	}

    /// <summary>
    /// At the start of every battle, initialize skill to set up its variables.
    /// </summary>
    public void Initialize()
    {
        data.Initialize();
    }

    public virtual SkillUsableResult IsUsable(BattleActor user)
    {
        if (CurrentCooldown > 0)
            return SkillUsableResult.IN_COOLDOWN;
        else if (user.Stack < data.Cost)
            return SkillUsableResult.NOT_ENOUGH_STACKS;
        else if (user is BattleCharacter character && !character.TurnActive)
            return SkillUsableResult.CHARACTER_NOT_ACTIVE;

		if (data.IsUsableAdditional(user))
		{
			return SkillUsableResult.USABLE;
		}
		else
		{
			return SkillUsableResult.SPECIAL_REQUIREMENT_NOT_MET;
		}
    }

	// Encapsulates Execute. Actual skill effects are implemented in Execute.
    public bool Use(BattleFieldData bF, BattleInteractionData bI)
    {
        if (IsUsable(bI.user) == SkillUsableResult.USABLE)
        {
            data.Execute(bF, bI);
            bI.user.ChangeStack(-data.Cost);
            CurrentCooldown = data.Cooldown;
            return true;
        }
        return false;
    }

    public int EstimateDamage(BattleActor user, BattleActor target)
    {
        return data.EstimateDamage(user, target);
    }

    public virtual bool Miss(BattleActor user, BattleActor target)
    {
        if (IsUsable(user) == 0)
        {
            user.ChangeStack(-data.Cost);
            CurrentCooldown = data.Cooldown;
            return true;
        }
        return false;
    }

    public string GetDescription(BattleFieldData bF, BattleCharacter user)
    { 
        return data.Description(bF, user);
    }

    public SkillElement GetDisplayElement(BattleFieldData bF, BattleCharacter user)
    {
        return data.GetDisplayElement(bF, user);
    }

    /// <summary>
    /// For skills that use TargettingType.CUSTOMWINDOW, instances and returns the appropriate .tscn
    /// </summary>
    /// <returns>The UI window that the skill uses to control itself.</returns>
    public virtual SkillCustomWindow GetWindow() { return data.GetCustomWindow(); }
}
