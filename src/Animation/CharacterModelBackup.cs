using Godot;
using System;

public class CharacterModelBackup : Node2D
{
    private AnimationPlayer blinkPlayer;
    [Export] private float blinkSpeed = 5;
    private float blinkNext;
    public bool blink = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        blinkPlayer = GetNode<AnimationPlayer>("BlinkAnimator");
        blinkNext = (GD.Randf() + 0.2f) * blinkSpeed;
    }

    public override void _Process(float delta)
    {
        if (blink) Blink(delta);
    }

    public void Blink(float delta)
    {
        if (blinkNext <= 0)
        {
            blinkPlayer.Play("Blink");
            blinkNext = (GD.Randf() + 0.2f) * blinkSpeed;
        }
        else
            blinkNext -= delta;
    }

    // public void ChangeExpression(string expression)
    // {
        
    // }
}
