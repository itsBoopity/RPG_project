using Godot;
using System;

public class AnimatedSpriteOneOff : AnimatedSprite
{
    public override void _Ready()
    {
        Play();
    }

    public override void _ExitTree()
    {
        QueueFree();
    }

    public void DeleteMe()
    {
        QueueFree();
    }
}
