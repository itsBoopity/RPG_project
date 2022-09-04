using Godot;
using System;

public class CustomizationColorTab : Node2D
{
    private PlayerEnum currentColorTab = PlayerEnum.Skin;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PackedScene buttonScene = GD.Load<PackedScene>("res://Objects/UI/Customization/CustomizationColorButton.tscn");

        foreach (PlayerColor i in Enum.GetValues(typeof(PlayerColor)))
        {
            CustomizationColorButton instance = buttonScene.Instance<CustomizationColorButton>();
            instance.Initialize(i);
            GetNode("PlayerColor").AddChild(instance);
        }

        foreach (PlayerSkin i in Enum.GetValues(typeof(PlayerSkin)))
        {
            CustomizationColorButton instance = buttonScene.Instance<CustomizationColorButton>();
            instance.Initialize(i);
            GetNode("PlayerSkin").AddChild(instance);
        }

        foreach (PlayerEyeColor i in Enum.GetValues(typeof(PlayerEyeColor)))
        {
            CustomizationColorButton instance = buttonScene.Instance<CustomizationColorButton>();
            instance.Initialize(i);
            GetNode("PlayerEyeColor").AddChild(instance);
        }

    }

    public void SetTab(PlayerEnum type)
    {
        currentColorTab = type;
        foreach (Node i in GetChildren())
        {
            ((CanvasItem)i).Hide();
        }
        if (type != PlayerEnum.Scruff)
            GetNode<CanvasItem>("TopLabel").Show();

        GetNode<Label>("TopLabel").Text = "Color";
        if (type == PlayerEnum.BeardColor || type == PlayerEnum.HairColor)
            GetNode<CanvasItem>("PlayerColor").Show();
        else if (type == PlayerEnum.Skin)
            GetNode<CanvasItem>("PlayerSkin").Show();

        if (type == PlayerEnum.Face)
        {
            GetNode<CanvasItem>("BottomLabel").Show();
            GetNode<CanvasItem>("PlayerColor").Show();
            GetNode<CanvasItem>("PlayerEyeColor").Show();
            GetNode<Label>("TopLabel").Text = "Eyebrow Color";
            GetNode<Label>("BottomLabel").Text = "Eye Color";
        }
    }
    public void ChangeOption(CustomizationColorButton caller)
    {
        if (currentColorTab == PlayerEnum.Face)
        {
            if (caller.GetParent().Name == "PlayerColor")
                GetNode<CharacterCreator>("../../").ChangeOption(PlayerEnum.BrowColor, caller.index);
            else
                GetNode<CharacterCreator>("../../").ChangeOption(PlayerEnum.EyeColor, caller.index);
        }

        GetNode<CharacterCreator>("../../").ChangeOption(currentColorTab, caller.index);
    }
}