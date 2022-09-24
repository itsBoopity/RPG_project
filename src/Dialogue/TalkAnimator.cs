using Godot;
using System;

public class TalkAnimator : AnimationPlayer
{
    public void RandomSpeed()
    {
        this.PlaybackSpeed = 1f + GD.Randf() * 0.7f;
    }
}
