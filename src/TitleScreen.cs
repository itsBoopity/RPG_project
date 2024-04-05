using System.Reflection.Metadata.Ecma335;
using Godot;

public partial class TitleScreen : Control, IMainScene
{
    public MainSceneEnum Type => MainSceneEnum.TITLESCREEN;
    public bool Serializable => false;

    public void NewGame()
    {
        GameData.Instance.NewSave();
        SceneManager.EnterDungeon(GD.Load<DungeonSetup>("res://Resources/Dungeon/Setup/TestSetup.tres"));
    }

    public void OpenLoadWindow()
    {
        //Show the window with save files
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
