using System.Collections.Generic;

public interface IBattleActor
{
    public string DisplayName { get; }
    public int Level { get; }
    public int MaxHealth { get; }
    public int Health { get; }
    public int Stack { get; }
    
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

    /// <summary>
    /// Sustains finalDamage amount of damage. The passed value should be the final calculated damage, as this method does not adjust it in any way.
    /// </summary>
    public void SustainDamage(int finalDamage);

    /// <summary>
    /// Updates stack stat.
    /// </summary>
    /// <param name="delta"></param>
    public void ChangeStack(int delta);
}