using Godot;
using System;

public class MonsterModel : Node2D
{
    AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void targetHit(MonsterTarget target)
    {
        //Send signal to battle engine of target value// or check for if playerTurn/TargettingUI is enabled here.
        GD.Print(Name + " got hit at " + target.GetParent().Name + " = " + target.Name);
    }

    public void Animate(string name)
    {
        animationPlayer.Stop();
        animationPlayer.Play(name);
    }
}
