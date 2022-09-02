using Godot;
using System;

public class CustomizationColorButton : SquareButton
{
    public int index;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
    }

    public void Initialize(PlayerSkin color)
    {
        ((ShaderMaterial)GetChild<ColorRect>(0).Material).SetShaderParam("color", new Color("C39D8C"));
        Utility.SetShaderColor(GetChild<ColorRect>(0), color);
        this.index = (int)color;
    }
    public void Initialize(PlayerColor color)
    {
        ((ShaderMaterial)GetChild<ColorRect>(0).Material).SetShaderParam("color", new Color("573439"));
        Utility.SetShaderColor(GetChild<ColorRect>(0), color);
        this.index = (int)color;
    }
    public void Initialize(PlayerEyeColor color)
    {
        ((ShaderMaterial)GetChild<ColorRect>(0).Material).SetShaderParam("color", new Color("5C617B"));
        Utility.SetShaderColor(GetChild<ColorRect>(0), color);
        this.index = (int)color;
    }

    public override void OnPress()
    {
        GetNode<CustomizationColorTab>("../../").ChangeOption(this);
    }
}
