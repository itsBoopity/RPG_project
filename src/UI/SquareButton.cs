using Godot;
using System;

public abstract class SquareButton : TextureButton
{
    private Vector2 originalSize;
    private float expandFactor = 1.15f;
    private float midExpandFactor = 1.07f;
    public override void _Ready()
    {
        //selector = GetNode<Selector>("../../Selector");
        originalSize = RectScale;
    }

    public void ScaleBack()
    {
        RectScale = originalSize;
    }

    public void ScaleMid()
    {
        RectScale = originalSize * midExpandFactor;
    }

    public void ScaleUp()
    {
        RectScale = originalSize * expandFactor;
    }

    abstract public void OnPress();
}
