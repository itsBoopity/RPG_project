using Godot;
using System;

public class SidebarHighlight : Sprite
{
    Vector2 originalScale;
    Control following;
    AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        originalScale = Scale;
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Visible = false;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        if (following != null)
            GlobalPosition = new Vector2 (0,following.RectGlobalPosition.y);
    }

    public void ScaleDown()
    {
        this.Scale = new Vector2(originalScale.x * 0.95f , originalScale.y);
    }

    public void ScaleBack()
    {
        this.Scale = originalScale;
    }

    public void Focus(Control toFocus)
    {
        Visible = true;
        following = toFocus;
        animationPlayer.Play("PlayHighlight");
    }
}
