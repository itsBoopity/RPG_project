using System;
using System.Collections.Generic;

// Class that holds info about a battle, such as monsters, extra conditions, win conditions etc.
// Inherits from GodotObject so it can be passed in native Godot functions.
[Serializable]
public partial class BattleSetup: Godot.GodotObject
{
    private List<int> monsterIds = new();
    public List<int> MonsterIds { get {return monsterIds;} }

    private BattleWinCondition winCondition = BattleWinCondition.DEFEAT_ALL;
    public BattleWinCondition WinCondition { get {return winCondition;} }

    ~BattleSetup() { this.Free(); }

    public BattleSetup AddMonsterId(int monsterId)
    {
        monsterIds.Add(monsterId);
        return this;
    }


    //BattleCondition[] battleConditions;
    // public void ExecuteConditions(BattleEngine battleEngine)
    // {
    //     foreach (BattleCondition i in battleConditions)
    //     {
    //         i.Execute(battleEngine)
    //     }
    // }
}