using Godot;
using System;

public class Tab : TextureButton
{
    Vector2 originalPosition;
    Vector2 originalScale;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        originalPosition = this.RectPosition;
        originalScale = this.RectScale;
    }

    public void PushUp()
    {
        this.RectPosition = originalPosition - new Vector2(0,10).Rotated(GetRotation());
    }

    public void PushBack()
    {
        this.RectPosition = originalPosition;
    }

    public void ScaleDown()
    {
        this.RectScale = originalScale * 0.98f;
    }

    public void ScaleBack()
    {
        this.RectScale = originalScale;
    }
}
