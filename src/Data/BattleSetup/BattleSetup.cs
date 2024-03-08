using Godot;
using Godot.Collections;

// Class that holds info about a battle, such as monsters, extra conditions, win conditions etc.
// Inherits from GodotObject so it can be passed in native Godot functions.
[GlobalClass]
public partial class BattleSetup: Resource
{

    [Export]
    public Array<PackedScene> monsters { get; set; }

    private BattleWinCondition winCondition = BattleWinCondition.DEFEAT_ALL;
    public BattleWinCondition WinCondition { get {return winCondition;} }

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