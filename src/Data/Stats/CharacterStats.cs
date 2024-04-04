using Godot;
// using Newtonsoft.Json;

// [JsonObject(MemberSerialization.OptIn)]
public partial class CharacterStats: BattleActorStats
{
    
    [Export]
    // [JsonProperty]
    public CharacterEnum Who {get; set;}

    public CharacterStats() {}

}