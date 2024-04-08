using Godot;

public partial class TitleScreen : Control, IMainScene
{
    public MainSceneEnum MainSceneType => MainSceneEnum.TITLESCREEN;
    public bool MainSceneSerializable => false;
    public string MainSceneDescription => "";


    public void NewGame()
    {
        Global.ResetTimeSinceLastSave();
        GameData.Instance.NewSave();
        SceneManager.EnterDungeon(GD.Load<DungeonSetup>("res://Resources/Dungeon/Setup/TestSetup.tres"));
    }

    public void OpenLoadWindow()
    {
        GetNode<SaveWindow>("%SaveWindow").Open();
    }

    public void OpenSettingsWindow()
    {
        GetNode<SettingsWindow>("%SettingsWindow").Open();
    }
    
    public void Exit()
    {
        GetTree().Quit();
    }
}
