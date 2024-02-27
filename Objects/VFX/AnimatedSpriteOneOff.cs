using Godot;
using System;

public partial class AnimatedSpriteOneOff : AnimatedSprite2D
{
    [Export] AudioStreamWav[] sfxSrc;
    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("Audio").Stream = sfxSrc[GD.Randi() % sfxSrc.Length];
        GetNode<AudioStreamPlayer>("Audio").PitchScale = 0.8f + GD.Randf() * 0.4f;
        GetNode<AudioStreamPlayer>("Audio").Play();
        Play();
    }

    public override void _ExitTree()
    {
        QueueFree();
    }

    public void DeleteMe()
    {
        EmitSignal(SignalName.AnimationFinished);
        QueueFree();
    }
}
