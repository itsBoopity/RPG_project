using Godot;
using System.Collections.Generic;

public abstract partial class BattleActor: Node2D
{
    [Signal]
    public delegate void TookDamageEventHandler(BattleActor who, BattleActor damageDealer, int damage);

    public string DisplayName { get { return Tr(Stats.Name); } }
    public int Level { get { return Stats.Level; } }
    public int MaxHealth { get { return Stats.MaxHealth; } }
    public int Health { get { return Stats.Health; } private set { Stats.Health = value; } }
    public int Stack { get; private set;} = 0;
    private int Attack { get { return Stats.Attack; } }
    private int Defense { get { return Stats.Defense; } }
    private int Speed { get { return Stats.Speed; } }
    
    public bool TurnActive { get; set; } = true;
    public List<BattleSkill> Skills { get; set; }
    public List<int> Statuses { get; }
    private BattleActorStats stats;
    

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
                Skills.Add(new BattleSkill(data));
            }
        }
    }

    public BattleActor() {}

    /// <summary>
    /// Get final Attack stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetAttack()
    {
        return Attack;
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
    }

    /// <summary>
    /// Updates stack stat.
    /// </summary>
    /// <param name="delta"></param>
    public void ChangeStack(int delta)
    {
        Stack += delta;
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