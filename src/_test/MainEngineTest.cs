using Godot;
using System;

public class MainEngineTest : MainEngine
{
    public void CharacterCreatorTest()
    {
        gameData.newSave();
        AddChild(GD.Load<PackedScene>("res://Scenes/CharacterCreator.tscn").Instance());
    }

    public void DialogueTest()
    {
        gameData.newSave();
        DialogueEngine dialogueEngine = GD.Load<PackedScene>("res://Scenes/DialogueEngine.tscn").Instance<DialogueEngine>();
        AddChild(dialogueEngine);
        dialogueEngine.LoadDialogue("res://Dialogue/test.txt");
    }

    public void BattleEngineTest()
    {
        gameData.newSave();
        BattleEngine battleEngine = GD.Load<PackedScene>("res://Scenes/BattleEngine.tscn").Instance<BattleEngine>();
        AddChild(battleEngine);
        battleEngine.Initiate(this, new BattleSetup());
    }

    public void SaveLoadTest()
    {
        gameData.newSave();
        gameData.avaData.hair = PlayerHair.FRINGYMID;
        gameData.playerCharacter.who = CharacterEnum.NULL;
        gameData.playerCharacter.HP = 69;
        gameData.Save("testSave.dat");
        gameData.Load("testSave.dat");
    }
}
