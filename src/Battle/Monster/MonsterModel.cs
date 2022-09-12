using Godot;
using System;

public class MonsterModel : Node2D
{
    private AnimationPlayer animationPlayer;
    private Control boundary;

    [Signal] delegate void Hit(MonsterTarget appendage);
    [Signal] delegate void Miss();

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        boundary = GetNode<Control>("BoundaryBox");
    }

    public Vector2 GetBoundary()
    {
        if (boundary == null) throw new Exception("MonsterModel.GetBoundary called before model got added to the scenetree");
        return boundary.RectSize;
    }

    public void TargetMiss() { EmitSignal("Miss"); }

    public void TargetHit(MonsterTarget target) { EmitSignal("Hit", target);  }

    public void Animate(string name)
    {
        animationPlayer.Stop();
        animationPlayer.Play(name);
    }
}
