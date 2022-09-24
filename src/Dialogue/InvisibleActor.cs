using Godot;
using System;

public class InvisibleActor : CharacterModel
{
    public override void _Ready()
    {
        SetProcess(false);
        bleep = GetNode<AudioStreamPlayer2D>("Bleep");
        talkAnimator = GetNode<AnimationPlayer>("TalkAnimator");
    }

    public InvisibleActor SetVoice(AudioStream audio)
    {
        talkAnimator.Stop();
        bleep.Stream = audio;
        return this;
    }

    public override void Talk() 
    {
        if (bleep.Stream != null)
        {
            talkAnimator.GetAnimation("Talk").Loop = true;
            talkAnimator.Stop();
            talkAnimator.Play("Talk");
        }
    }
    public override void StopTalk()
    {
        talkAnimator.GetAnimation("Talk").Loop = false;
    }
}
