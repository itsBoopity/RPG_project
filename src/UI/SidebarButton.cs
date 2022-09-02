using Godot;
using System;

public abstract class SidebarButton : Button
{
    SidebarHighlight highlight;
    Vector2 originalScale;
    
    public override void _Ready()
    {
        highlight = GetNode<SidebarHighlight>("../../SidebarHighlight");
        originalScale = RectScale;
    }

    public void OnFocus()
    {
        this.GrabFocus();
        highlight.Focus(this);
        
    }

    public void OnHover()
    {
        if (!this.HasFocus())
            OnFocus();
    }

    public void ScaleDown()
    {
        this.RectScale = new Vector2(originalScale.x * 0.97f , originalScale.y);
        highlight.ScaleDown();
    }

    public void ScaleBack()
    {
        this.RectScale = originalScale;
        highlight.ScaleBack();
    }

    public void Continue()
    {
        //Load Save or smth
    }

    public void NewGame()
    {
        GetNode<MainEngine>("/root/MainEngine").gameData.newSave();
        GetNode<MainEngine>("/root/MainEngine").ChangeScene("res://Scenes/CharacterCreator.tscn");
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
