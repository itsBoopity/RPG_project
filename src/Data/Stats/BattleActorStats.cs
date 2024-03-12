using Godot;
using Godot.Collections;

public partial class BattleActorStats: Resource
{
    [Export]
    public string Name { get; set; }
    [Export]
    public int Level { get; set; }
    [Export]
    public int Health { get; set; }
    [Export]
    public int MaxHealth { get; set; }
    [Export]
    public int Attack { get; set; }
    [Export]
    public int Defense { get; set; }
    [Export]
    public int Speed { get; set; }
    [Export]
    public Array<BattleSkillData> Skills { get; set; }
    public BattleActorStats() {}

    public virtual void OnDefeated(BattleActor damageDealer) {}
}