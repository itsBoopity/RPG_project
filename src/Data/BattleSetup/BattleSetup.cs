using Godot;
using Godot.Collections;
using Newtonsoft.Json;

// Class that holds info about a battle, such as monsters, extra conditions, win conditions etc.
// Inherits from GodotObject so it can be passed in native Godot functions.
[GlobalClass]
public partial class BattleSetup: Resource
{

    [Export]
    [JsonProperty]
    public Array<PackedScene> Monsters { get; set; }

    [JsonProperty]
    public BattleWinCondition WinCondition { get; } = BattleWinCondition.DEFEAT_ALL;

    public BattleSetup() {}

    //BattleCondition[] battleConditions;
    // public void ExecuteConditions(BattleEngine battleEngine)
    // {
    //     foreach (BattleCondition i in battleConditions)
    //     {
    //         i.Execute(battleEngine)
    //     }
    // }
}