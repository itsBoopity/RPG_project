using Godot;
using System;

public class BattleNarration : Node2D
{
    private AnimationPlayer animationPlayer;
    private Label label;

    public override void _Ready()
    {
        label = GetNode<Label>("Label");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _ExitTree()
    {
        animationPlayer.Stop();
        label.Text = "";
    }

    public void ShowText(string text)
    {
        label.Text = text;
        animationPlayer.Stop();
        animationPlayer.Play("ShowText");
    }

    public void TimeOut()
    {
        label.Text = "Ran out of time!";
        animationPlayer.Stop();
        animationPlayer.Play("ShowText");
    }
}
