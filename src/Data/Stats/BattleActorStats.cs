using Godot;
using Godot.Collections;
// using Newtonsoft.Json;

// [JsonObject(MemberSerialization.OptIn)]
public partial class BattleActorStats: Resource
{
    [Export]
    // [JsonProperty]
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

    /// <summary>
    /// "Inherent" element of the character. Can be used by skills that inherit user's element. 
    /// </summary>
    [Export]
    public SkillElement Element { get; set; }

    [Export]
    public Dictionary<string, float> ElementalAffinity { get; set; } = new() {
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
        return ElementalAffinity[element.ToString()];
    }

    public virtual void OnDefeated(BattleActor damageDealer) {}
}