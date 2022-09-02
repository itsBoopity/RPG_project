using Godot;
using System;

public class CharacterModel : Node2D
{
    [Export] NodePath headPath = null;
    [Export] NodePath mouthPath = null;
    private float blinkSpeed = 5;
    private float talkSpeed = 1;
    private AudioStreamPlayer2D bleep;
    private AnimationPlayer blinkAnimator;
    private AnimatedSprite head;
    private AnimatedSprite mouth;
    private float blinkNext;
    private float talkLength;
    private float talkNext;
    public bool blink = true;

    public override void _Ready()
    {
        mouth = GetNode<AnimatedSprite>(mouthPath);
        head = GetNode<AnimatedSprite>(headPath);
        bleep = GetNode<AudioStreamPlayer2D>("Bleep");
        blinkAnimator = GetNode<AnimationPlayer>("BlinkAnimator");
        blinkNext = (GD.Randf() + 0.2f) * blinkSpeed;
    }

    public override void _Process(float delta)
    {
        if (blink) Blink(delta);
        TalkInternal(delta);
    }

    public void Blink(float delta)
    {
        if (blinkNext <= 0)
        {
            blinkAnimator.Play("Blink");
            blinkNext = (GD.Randf() + 0.2f) * blinkSpeed;
        }
        else
            blinkNext -= delta;
    }

    public void StopTalk()
    {
        talkLength = 0;
    }

    public void Talk(int length) //Character length of the sentence to be said
    {
        talkLength = length * 0.01f;
        talkNext = -2f;
    }

    private void TalkInternal(float delta)
    {
        if (talkLength > 0)
        {
            if (talkNext < 0)
            {
                head.Frame = 0;
                mouth.Frame = 0;
            }
            if (talkNext < - talkSpeed * 0.1f)
            {
                bleep.Play();
                talkNext = GD.Randf() * talkSpeed * 0.05f + 0.05f;
                head.Frame = 1;
                mouth.Frame = 1;
            }

            talkNext -= delta;
            talkLength -= delta;
        }
        else
        {
            head.Frame = 0;
            mouth.Frame = 0;
        }
    }

    public void ChangeExpression(string expression)
    {
        
    }
}
