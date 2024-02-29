using Godot;

/// <summary>
/// Plays the turn announcement label fade in that lasts for as long as the enemy takes turns, then fades out.
/// </summary>
public partial class TurnAnnouncement : Node
{
    Control label;
    Tween tweenModulation;
    Tween tweenPosition;

    public override void _Ready()
    {
        label = GetNode<Control>("Label");
        tweenModulation = CreateTween(); tweenModulation.Stop();
        tweenPosition = CreateTween(); tweenPosition.Stop();
    }

    // Need to kill the tweens on exit program?
    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest) {
            if (tweenModulation != null) tweenModulation.Kill();
            if (tweenModulation != null) tweenPosition.Kill();
        }
    }

    public void Play()
    {
        tweenModulation.Kill();
        tweenPosition.Kill();
        tweenModulation = CreateTween();
        tweenPosition = CreateTween();

        label.SelfModulate = new Color (1,1,1,0);
        tweenModulation.TweenProperty(label, "self_modulate", new Color (1,1,1,1), 0.1f);
        
        label.Position = new Vector2(300, 0);
        tweenPosition.TweenProperty(label, "position:x", -200, 0.2f);
        tweenPosition.TweenProperty(label, "position", new Vector2(-350, 0), 10f);
    }
    public void Stop()
    {
        tweenModulation.Kill();
        tweenPosition.Kill();
        tweenModulation = CreateTween();
        tweenPosition = CreateTween();

        tweenPosition.TweenProperty(label, "position:x", -800, 0.2f);
        tweenModulation.TweenInterval(0.1f);
        tweenModulation.TweenProperty(label, "self_modulate", new Color (1,1,1,0), 0.1f);
    }
}
