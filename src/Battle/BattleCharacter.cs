using System.Collections.Generic;

/// <summary>
/// A class representing a player character during battles.
/// </summary>
public class BattleCharacter: IBattleActor
{
    private CharacterEnum _who;
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
    private bool _turnActive;

    public CharacterEnum Who { get {return _who;} }
    public string Name => _name;
    public int Level => _level;
    public int MaxHealth => _maxHealth;
    public int Health { get { return _health;} set { _health = value; }}
    public int Stack { get { return _stack;} set { _stack = value; }}
    public List<BattleSkill> Skills => _skills;
    public List<int> Statuses => _statuses;
    public bool TurnActive { get { return _turnActive;} set { _turnActive = value; }}

    public BattleCharacter( CharacterEnum who,string name, int level, int health, int maxHealth,
                            int attack, int defense, int speed, List<BattleSkill> skills)
    {
        _who = who;
        _name = name;
        _level = level;
        _health = health;
        _maxHealth = maxHealth;
        _attack = attack;
        _defense = defense;
        _speed = speed;
        _skills = skills;
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

}