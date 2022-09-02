using Godot;
using System;
using System.Collections.Generic;

// Class that holds info about a battle, such as monsters, extra conditions, win conditions etc.
[Serializable]
public class BattleSetup
{
    List<string> monsterID;
    List<Vector2> monsterPositions; //their position on the monster field

    //BattleCondition[] battleConditions;
    // public void ExecuteConditions(BattleEngine battleEngine)
    // {
    //     foreach (BattleCondition i in battleConditions)
    //     {
    //         i.Execute(battleEngine)
    //     }
    // }
}