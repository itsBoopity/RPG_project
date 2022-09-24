using Godot;
using System;

public class AdvanceIcon : Sprite
{
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Hide();
    }

    public void ShowMe() { animationPlayer.Stop(); animationPlayer.Play("Show"); }
    public void HideMe() { animationPlayer.Stop(); animationPlayer.Play("Hide"); }
}
