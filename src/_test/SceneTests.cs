using Godot;
using System;

public class SceneTests: Node
{
    public void CharacterCreatorTest()
    {
        Global.data.newSave();
        GetNode<Global>("/root/Global").ChangeScene("res://Scenes/CharacterCreator.tscn");
    }

    public void DialogueTest()
    {
        Global.data.newSave();
        DialogueEngine dialogue = Utility.InstanceDialogue();
        AddChild(dialogue);
        dialogue.LoadDialogue("res://Dialogue/test.txt");
    }

    public void BattleEngineTest()
    {
        Global.data.newSave();
        BattleEngine battleEngine = Utility.InstanceBattle();
        AddChild(battleEngine);
        BattleSetup testSetup = new BattleSetup();
        testSetup.monsterID.Add(0);
        testSetup.monsterID.Add(0);
        battleEngine.Initiate(testSetup);
    }

    public void SaveLoadTest()
    {
        Global.data.newSave();
        Global.data.Save("testSave.dat");
        Global.data.Load("testSave.dat");
    }
}
