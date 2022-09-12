using Godot;
using System;

public class CharacterModel : Node2D
{
    private float blinkSpeed = 5;
    protected AudioStreamPlayer2D bleep;
    private AnimationPlayer blinkAnimator;
    protected AnimationPlayer talkAnimator;
    private float blinkNext;
    public bool blink = true;

    private float dialogueSpeed;

    public override void _Ready()
    {
        bleep = GetNode<AudioStreamPlayer2D>("Bleep");
        blinkAnimator = GetNode<AnimationPlayer>("BlinkAnimator");
        talkAnimator = GetNode<AnimationPlayer>("TalkAnimator");
        blinkNext = (GD.Randf() + 0.2f) * blinkSpeed;

        dialogueSpeed = Global.settings.dialogueSpeed;
    }

    /// <summary>
    /// pathToImage is the full path to the outfit
    /// </summary>
    public void SetOutfit(string pathToImage)
    {
        GetNode<Sprite>("Body/Outfit").Texture = GD.Load<ImageTexture>(pathToImage);
    }

    public override void _Process(float delta)
    {
        if (blink) Blink(delta);
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

    virtual public void Talk() 
    {
        talkAnimator.GetAnimation("Talk").Loop = true;
        talkAnimator.Stop();
        talkAnimator.Play("Talk");
    }
    virtual public void StopTalk()
    {
        talkAnimator.GetAnimation("Talk").Loop = false;
    }

    public void ChangeExpression(string expression)
    {
        
    }
}
