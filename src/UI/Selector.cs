using Godot;
using System;

public class Selector : Node2D
{
    private BaseButton following;
    private Vector2 originalScale;

    public override void _Ready()
    {
        originalScale = Scale;
    }

    public override void _Process(float delta)
    {
        if (following != null)
        {
            this.GlobalPosition = following.RectGlobalPosition;
            this.Scale = originalScale * following.RectScale.x;
        }
    }

    public void Select(BaseButton button)
    {
        following = button;
    }
}
