using Godot;

public partial class SceneTests : Node
{
    public void BattleEngineTest()
    {
        GameData.Instance.newSave();
        BattleEngine battleEngine = Utility.InstanceBattle();
        AddChild(battleEngine);
        BattleSetup testSetup = new();
        testSetup
            .AddMonsterId(0)
            .AddMonsterId(0);
        battleEngine.Initiate(testSetup);
    }

    public void SaveLoadTest()
    {
        GameData.Instance.newSave();
        GameData.Instance.Save("testSave.dat");
        GameData.Instance.Load("testSave.dat");
    }
}
