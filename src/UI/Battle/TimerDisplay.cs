using Godot;

/// <summary>
/// UI Element that displays the remaining time of CustomTimer specified by Export.
/// </summary>
public partial class TimerDisplay : Node
{
    private ProgressBar bar;
    private Label label;
    private Tween tween;

    [Export]
    private CustomTimer timer;
    public override void _Ready()
    {
        bar = GetNode<ProgressBar>("Bar");
        label = GetNode<Label>("Label");
        timer.Stopped += Stop;
        timer.Timeout += Stop;
        timer.Started += StartTimer;
        SetProcess(false);
    }

    public override void _Process(double delta)
    {
        // GD.Print(timer.TimeLeft);
        label.Text = timer.TimeLeft.ToString("F1");
        bar.Value = timer.TimeLeft;
    }
    
    private void Stop()
    {
        if (tween.IsRunning())
        {
            tween.Kill();
        }
        SetProcess(false);
    }
    private void StartTimer(double length)
    {
        bar.MaxValue = length;
        tween = GetTree().CreateTween();
        tween.TweenProperty(bar, "value", length, 0.1f)
            .SetEase(Tween.EaseType.InOut);
        tween.TweenCallback(Callable.From(() => SetProcess(true)));
    }
}
