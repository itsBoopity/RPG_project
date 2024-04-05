using Godot;
using System.Collections.Generic;

public abstract partial class BattleActor: Node2D
{
    [Signal]
    public delegate void TookDamageEventHandler(BattleActor who, BattleActor damageDealer, int damage);
    [Signal]
    public delegate void StatsChangedEventHandler(BattleActor who);

    public string DisplayName { get { return Tr(Stats.Name); } }
    public int Level { get { return Stats.Level; } }
    public int MaxHealth { get { return Stats.MaxHealth; } }
    public int Health { get { return Stats.Health; } private set { Stats.Health = value; } }
    public int Stack { get; private set;} = 0;
    /// <summary>
    /// "Inherent" element of the character. Can be used by skills that inherit user's element. 
    /// </summary>
    public SkillElement Element { get { return Stats.Element; } }
    private int Strength { get { return Stats.Strength; } }
    private int Intelligence { get { return Stats.Intelligence; } }
    private int Defense { get { return Stats.Defense; } }
    private int Speed { get { return Stats.Speed; } }
    
    public bool TurnActive
    {
        get { return turnActive; }
        set
        {
            turnActive = value;
            EmitSignal(SignalName.StatsChanged, this);
        } 
    }
    public List<BattleSkill> Skills { get; set; }
    public List<int> Statuses { get; }
    private BattleActorStats stats;
    private bool turnActive = true;
    

    [Export]
    protected BattleActorStats Stats
    {
        get
        {
            return stats;
        }
        set
        {
            stats = (BattleActorStats)value.Duplicate();
            Skills = new();
            foreach (BattleSkillData data in stats.Skills)
            {
                Skills.Add(new BattleSkill((BattleSkillData)data.Duplicate(true)));
            }
        }
    }

    public BattleActor() {}

    /// <summary>
    /// Get final Strength stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetStrength()
    {
        return Strength;
    }

    /// <summary>
    /// Get final Intelligence stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetIntelligence()
    {
        return Intelligence;
    }


    /// <summary>
    /// Get final Defense stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetDefense()
    {
        return Defense;
    }
    /// <summary>
    /// Get final Speed stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetSpeed()
    {
        return Speed;
    }

    /// <summary>
    /// Sustains finalDamage amount of damage. The passed value should be the final calculated damage, as this method does not adjust it in any way.
    /// </summary>
    public virtual void SustainDamage(BattleActor damageDealer, int finalDamage)
    {
        if (finalDamage < 0)
        {
            finalDamage = 0;
        }
        Health -= finalDamage;
        EmitSignal(SignalName.TookDamage, this, damageDealer, finalDamage);
        EmitSignal(SignalName.StatsChanged, this);
    }

    /// <summary>
    /// Returns coefficient of damage modification for element.
    /// </summary>
    public float GetAffinity(SkillElement element)
    {
        return stats.GetAffinity(element);
    }

    /// <summary>
    /// Updates stack stat.
    /// </summary>
    /// <param name="delta"></param>
    public void ChangeStack(int delta)
    {
        Stack += delta;
        EmitSignal(SignalName.StatsChanged, this);
    }

    /// <summary>
    /// Counts down parameters at the start of the turn: cooldowns, statusi etc.
    /// </summary>
    public void NextTurn()
    {
        TurnActive = true;
        foreach (BattleSkill skill in Skills)
            if (skill.CurrentCooldown > 0) skill.CurrentCooldown -= 1;
    }
}