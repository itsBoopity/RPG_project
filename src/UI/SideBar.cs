using Godot;
using System;

public partial class SideBar : Node
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetChild<Control>(1).GetChild<Control>(0).GrabFocus();
        //if (nosavedata) then Disable continue
    }

    public void Continue()
    {
        //Load Save or smth
    }

    public void NewGame()
    {
        Global.Data.newSave();
        GetNode<Global>("/root/Global").ChangeSceneToFile("res://Scenes/CharacterCreator.tscn");
    }

    public void LoadGame()
    {
        //Show the window with save files
    }

    public void Settings()
    {
        //Show settings window
    }
    
    public void Exit()
    {
        GetTree().Quit();
    }
}
