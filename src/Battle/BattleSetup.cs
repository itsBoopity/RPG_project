using Godot;
using System;
using System.Collections.Generic;

// Class that holds info about a battle, such as monsters, extra conditions, win conditions etc.
[Serializable]
public class BattleSetup: Godot.Object
{
    public List<int> monsterID = new List<int>();
    public BattleWinCondition winCondition = BattleWinCondition.DEFEAT_ALL;

    ~BattleSetup() { this.Free(); }

    //BattleCondition[] battleConditions;
    // public void ExecuteConditions(BattleEngine battleEngine)
    // {
    //     foreach (BattleCondition i in battleConditions)
    //     {
    //         i.Execute(battleEngine)
    //     }
    // }
}