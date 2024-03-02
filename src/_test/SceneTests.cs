using Godot;

public partial class SceneTests : Node
{
    public void BattleEngineTest()
    {
        GameData.Instance.newSave();
        BattleEngine battleEngine = Utility.InstanceBattle();
        AddChild(battleEngine);
        BattleSetup testSetup = new BattleSetup();
        testSetup.monsterID.Add(0);
        testSetup.monsterID.Add(0);
        battleEngine.Initiate(testSetup);
    }

    public void SaveLoadTest()
    {
        GameData.Instance.newSave();
        GameData.Instance.Save("testSave.dat");
        GameData.Instance.Load("testSave.dat");
    }
}
