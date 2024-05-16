using Godot;

public partial class CharacterModel : Node2D
{
    public bool blink = true;

    [Export]
    public Curve blinkCurve;

    private AnimationPlayer blinkAnimator;
    private Timer blinkTimer;

    private float dialogueSpeed;

    public override void _Ready()
    {
        blinkAnimator = GetNode<AnimationPlayer>("BlinkAnimator");
        blinkTimer = GetNode<Timer>("BlinkAnimator/BlinkTimer");
        RefreshBlinkTimer();

        dialogueSpeed = Global.Settings.dialogueSpeed;
    }

    public void RefreshBlinkTimer()
    {
        blinkTimer.Start(blinkCurve.Sample(GD.Randf()));
    }

    public void Blink()
    {
        blinkAnimator.Play("Blink");
        RefreshBlinkTimer();
    }
}
