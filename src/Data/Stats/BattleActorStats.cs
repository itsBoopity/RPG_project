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
    public int Strength { get; set; }
    [Export]
    public int Intelligence { get; set; }
    [Export]
    public int Defense { get; set; }
    [Export]
    public int Speed { get; set; }

    [Export]
    public SkillElement Element { get; set; }

    [Export(PropertyHint.Enum)]
    public Dictionary<string, float> elementalAffinity { get; set; } = new() {
        {SkillElement.NONE.ToString(), 1.0f},
        {SkillElement.BLUNT.ToString(), 1.0f},
        {SkillElement.PIERCE.ToString(), 1.0f},
        {SkillElement.SLASH.ToString(), 1.0f},
        {SkillElement.FIRE.ToString(), 1.0f},
        {SkillElement.ICE.ToString(), 1.0f},
        {SkillElement.LIGHTNING.ToString(), 1.0f}
    };

    [Export]
    public Array<BattleSkillData> Skills { get; set; }

    public BattleActorStats() {}
    public float GetAffinity(SkillElement element)
    {
        return elementalAffinity[element.ToString()];
    }

    public virtual void OnDefeated(BattleActor damageDealer) {}
}