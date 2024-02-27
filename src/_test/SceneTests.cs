using Godot;

public partial class SceneTests : Node
{
    public void BattleEngineTest()
    {
        Global.Data.newSave();
        BattleEngine battleEngine = Utility.InstanceBattle();
        AddChild(battleEngine);
        BattleSetup testSetup = new BattleSetup();
        testSetup.monsterID.Add(0);
        testSetup.monsterID.Add(0);
        battleEngine.Initiate(testSetup);
    }

    public void SaveLoadTest()
    {
        Global.Data.newSave();
        Global.Data.Save("testSave.dat");
        Global.Data.Load("testSave.dat");
    }
}
