using Godot;
using System;

public class TurnAnnouncement : Node2D
{
    Control label;
    SceneTreeTween tweenModulation;
    SceneTreeTween tweenPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        label = GetNode<Control>("Label");
        tweenModulation = GetTree().CreateTween(); tweenModulation.Stop();
        tweenPosition = GetTree().CreateTween(); tweenPosition.Stop();
    }

    public override void _Notification(int what)
    {
        if (what == MainLoop.NotificationWmQuitRequest)
        {
            tweenModulation.Kill();
            tweenPosition.Kill();
        }
    }

    public void Play()
    {
        tweenModulation.Kill();
        tweenPosition.Kill();
        tweenModulation = GetTree().CreateTween();
        tweenPosition = GetTree().CreateTween();

        label.SelfModulate = new Color (1,1,1,0);
        tweenModulation.TweenProperty(label, "self_modulate", new Color (1,1,1,1), 0.1f);
        
        label.RectPosition = new Vector2(-500, 0);
        tweenPosition.TweenProperty(label, "rect_position", new Vector2(0, 0), 0.2f);
        tweenPosition.TweenProperty(label, "rect_position", new Vector2(150, 0), 10f);
    }
    public void Stop()
    {
        tweenModulation.Kill();
        tweenPosition.Kill();
        tweenModulation = GetTree().CreateTween();
        tweenPosition = GetTree().CreateTween();


        tweenPosition.TweenProperty(label, "rect_position", new Vector2(600, 0), 0.2f);
        tweenModulation.TweenInterval(0.1f);
        tweenModulation.TweenProperty(label, "self_modulate", new Color (1,1,1,0), 0.1f);
    }
}
