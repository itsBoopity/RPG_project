using Godot;
using System;

public class CustomizationColorWindow : Node2D
{
    AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        PackedScene buttonScene = GD.Load<PackedScene>("res://Objects/UI/Customization/CustomizationColorButtonAll.tscn");

        foreach (PlayerSkin i in Enum.GetValues(typeof(PlayerSkin)))
        {
            CustomizationColorButtonAll instance = buttonScene.Instance<CustomizationColorButtonAll>();
            instance.Initialize(i);
            GetNode("HBoxContainer/Skin/Color").AddChild(instance);
        }

        foreach (PlayerColor i in Enum.GetValues(typeof(PlayerColor)))
        {
            CustomizationColorButtonAll instance = buttonScene.Instance<CustomizationColorButtonAll>();
            instance.Initialize(i);
            GetNode("HBoxContainer/Hair/Color").AddChild(instance);
        }

        foreach (PlayerEyeColor i in Enum.GetValues(typeof(PlayerEyeColor)))
        {
            CustomizationColorButtonAll instance = buttonScene.Instance<CustomizationColorButtonAll>();
            instance.Initialize(i);
            GetNode("HBoxContainer/Eye/Color").AddChild(instance);
        }

    }
    public void ChangeOption(CustomizationColorButtonAll caller)
    {
        if (caller.GetParent().GetParent().Name == "Skin")
            GetNode<CharacterCreator>("../../../").ChangeOption(PlayerEnum.Skin, caller.index);
        else if (caller.GetParent().GetParent().Name == "Hair")
            GetNode<CharacterCreator>("../../../").ChangeOption(PlayerEnum.AllHairColor, caller.index);
        else if (caller.GetParent().GetParent().Name == "Eye")
            GetNode<CharacterCreator>("../../../").ChangeOption(PlayerEnum.EyeColor, caller.index);
    }

    public void ShowAnimate()
    {
        animationPlayer.Stop();
        this.Visible = true;
        animationPlayer.Play("FadeIn");
    }
}