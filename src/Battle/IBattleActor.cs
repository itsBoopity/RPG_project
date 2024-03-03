using System.Collections.Generic;

public interface IBattleActor
{
    public string Name { get; }
    public int Level { get; }
    public int MaxHealth { get; }
    public int Health { get; set; }
    public int Stack { get; set; }
    public List<BattleSkill> Skills { get; }
    public List<int> Statuses { get; }

    /// <summary>
    /// Get final Attack stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetAttack();
    /// <summary>
    /// Get final Defense stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetDefense();
    /// <summary>
    /// Get final Speed stat calculated with every modifier from statuses applied to it.
    /// </summary>
    public int GetSpeed();
}