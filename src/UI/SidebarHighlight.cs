using Godot;
using System;

public partial class SidebarHighlight : Sprite2D
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
    public override void _Process(double delta)
    {
        if (following != null)
            GlobalPosition = new Vector2 (0,following.GlobalPosition.Y);
    }

    public void ScaleDown()
    {
        this.Scale = new Vector2(originalScale.X * 0.95f , originalScale.Y);
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
