using Godot;

public partial class TitleScreen : Control
{
    public void NewGame()
    {
        GameData.Instance.NewSave();
        SceneManager.Instance.ChangeSceneToFile("res://Scenes/DungeonEngine.tscn");
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
