using Godot;
using System;

public class CustomizationWindow : Control
{
    [Export] private PlayerEnum type = PlayerEnum.Hair;
    [Export] WindowLayout layout = WindowLayout.Big;
    AnimationPlayer animationPlayer;

    private enum WindowLayout {Big, TopHalf, BottomHalf, TopMore, BottomLess, TopThird, MiddleThird, BottomThird}

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play(layout.ToString());

        PackedScene buttonScene = GD.Load<PackedScene>("res://Objects/UI/Customization/CustomizationButton.tscn");

        for (int i=0; i < Utility.PlayerEnumSize(type); i++)
        {
            CustomizationButton toAdd = buttonScene.Instance<CustomizationButton>();
            GetNode("ScrollContainer/MarginContainer/Grid").AddChild(toAdd);
            toAdd.Initialize(type, i);
        }

        buttonScene.Dispose();
    }

    public void ShowAnimate()
    {
        this.Show();
        animationPlayer.Stop();
        animationPlayer.Play(layout.ToString());
    }
}
