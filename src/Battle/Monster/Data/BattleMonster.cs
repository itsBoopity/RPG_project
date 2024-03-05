using Godot;
using System.Collections.Generic;

public abstract partial class BattleMonster: Node2D, IBattleActor
{
    [Signal]
    public delegate void AttackedEventHandler(BattleMonster who, float appendageCoef);

    [Signal]
    public delegate void MissedEventHandler(BattleMonster who);

    public readonly MonsterId _id;
    protected MonsterVisuals visuals;
    private readonly string _name;
    private readonly int _level;
    private readonly int _maxHealth;
    private readonly int _attack;
    private readonly int _defense;
    private readonly int _speed;
    private int _health;
    private int _stack;
    private readonly List<BattleSkill> _skills;
    private readonly List<int> _statuses = new();

    public MonsterId Id { get {return _id;} }
    public string DisplayName => Tr(_name);
    public int Level => _level;
    public int MaxHealth => _maxHealth;
    public int Health { get { return _health;} }
    public int Stack { get { return _stack;} }
    public List<BattleSkill> Skills => _skills;
    public List<int> Statuses => _statuses;

    public bool targettingEnabled = false;
    private bool isDisappearing = false;

    // The default damage modifier that gets used when non targetting offensive skills are used 
    protected float defaultDamage = 0.5f;
    
    //Position of party member and skill index that is going to be used, decided when player turn starts
    protected int targetCharacter = 0;
    protected int targetSkill = 0;

    public bool IsDisappearing { get {return isDisappearing; } }
    protected BattleMonster(   MonsterId id, string name, int level, int health, int maxHealth,
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
    }

    public override void _Ready()
    {
        visuals = GetNode<MonsterVisuals>("%Visuals");
        visuals.UpdateHP(Health, MaxHealth);
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

    public void ChangeStack(int delta)
    {
        _stack += delta;
    }

    public void SustainDamage(int damage)
    {
        _health -= damage;
        visuals.UpdateHP(Health, MaxHealth);
        visuals.PlayDamage(damage);
    }
    public async void Defeated()
    {
        targettingEnabled = false;
        isDisappearing = true;
        await ToSignal(visuals.AnimateDefeat(), AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
    }

    public bool CanAct()
    {
        return !isDisappearing;
    }

    public Node ExecuteTurn(BattleEngine battleEngine)
    {
        Skills[targetSkill].Use(battleEngine, this, battleEngine.GetPartyMember(targetCharacter));
        string targetName = battleEngine.GetPartyMember(targetCharacter).DisplayName;
        battleEngine.Ui.ShowCenterMessage(Name + " uses " + Skills[targetSkill].name + " on " + targetName + "!");
        Node2D animation = Skills[targetSkill].GetAnimation();
        battleEngine.Ui.SpawnEffectParty(animation, battleEngine.GetPartyMember(targetCharacter));
        ExecuteTurnDecorate();
        return animation;
    }

    // play animation or other unique animations a Monster inherited class would want when it performs its turn.
    protected virtual void ExecuteTurnDecorate() {}

    public void AppendageHitCheck(MonsterAppendage target)
    {
        if (targettingEnabled) AppendageHit(target);
    }

    public void AppendageMissCheck()
    {
        if (targettingEnabled) AppendageMiss();
        
    }
    private void AppendageMiss()
    {
        EmitSignal(SignalName.Missed, this);
    }
    protected abstract void AppendageHit(MonsterAppendage target);
    public abstract void LoadUpcomingTurn(BattleEngine battleEngine);
}