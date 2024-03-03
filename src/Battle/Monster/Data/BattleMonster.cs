using Godot;
using System.Collections.Generic;

public abstract class BattleMonster: IBattleActor
{
    public readonly MonsterId _id;
    private readonly string _name;
    private readonly int _level;
    private readonly int _maxHealth;
    private readonly int _attack;
    private readonly int _defense;
    private readonly int _speed;
    private int _health;
    private int _stack;
    private readonly List<BattleSkill> _skills;
    private readonly List<int> _statuses;

    public MonsterId Id { get {return _id;} }
    public string Name => _name;
    public int Level => _level;
    public int MaxHealth => _maxHealth;
    public int Health { get { return _health;} set { _health = value; }}
    public int Stack { get { return _stack;} set { _stack = value; }}
    public List<BattleSkill> Skills => _skills;
    public List<int> Statuses => _statuses;

    public bool targettingEnabled = false;
    protected MonsterModel model = null;

    // The default damage modifier that gets used when non targetting offensive skills are used 
    protected float defaultDamage = 0.5f;
    
    //Position of party member and skill index that is going to be used, decided when player turn starts
    protected int targetCharacter = 0;
    protected int targetSkill = 0;

    public BattleMonster(   MonsterId id, string name, int level, int health, int maxHealth,
                            int attack, int defense, int speed, List<BattleSkill> skills)
    {
        _id = id;
        _name = name;
        _level = level;
        _health = health;
        _maxHealth = maxHealth;
        _attack = attack;
        _defense = defense;
        _speed = speed;
        _skills = skills;
        model = GD.Load<PackedScene>("res://Objects/Monster/" + this.GetType().Name + ".tscn").Instantiate<MonsterModel>();
        model.SetOwner(this);
    }

    public int GetAttack()
    {
        return _attack;
    }

    public int GetDefense()
    {
        return _defense;
    }

    public int GetSpeed()
    {
        return _speed;
    }

    public void Defeated()
    {
        targettingEnabled = false;
        //model.QueueFree();
        model.AnimateDefeat();
        model = null;
    }

    public MonsterModel GetModel()
    {
        if (model == null)
        {
            model = GD.Load<PackedScene>("res://Objects/Monster/" + this.GetType().Name + ".tscn").Instantiate<MonsterModel>();
            model.SetOwner(this);
        }
        return model;
    }

    public Node ExecuteTurn(BattleEngine battleEngine)
    {
        Skills[targetSkill].Use(battleEngine, this, battleEngine.GetPartyMember(targetCharacter));
        string targetName = battleEngine.GetPartyMember(targetCharacter).Name;
        battleEngine.Ui.ShowCenterMessage(Name + " uses " + Skills[targetSkill].name + " on " + targetName + "!");
        Node2D animation = Skills[targetSkill].GetAnimation();
        battleEngine.Ui.SpawnEffectParty(animation, battleEngine.GetPartyMember(targetCharacter));
        ExecuteTurnAdditional();
        return animation;
    }

    // play animation or other unique animations a Monster inherited class would want when it performs its turn.
    protected virtual void ExecuteTurnAdditional() {}
    public void NotifyBattleEngine(float targetEfficiency)
    {
        battleEngine.ExecuteSkill(this, targetEfficiency);
    }

    public void Hit(MonsterTarget target)
    {
        if (targettingEnabled) TargetHit(target);
    

    }
    public void TargetMiss()
    {
        if (targettingEnabled) battleEngine.PlayerMissed(this);
    }

    protected abstract void TargetHit(MonsterTarget target);
    public abstract void LoadUpcomingTurn(BattleEngine battleEngine);
}