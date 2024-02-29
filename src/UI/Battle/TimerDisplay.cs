using Godot;

public partial class TimerDisplay : Node
{
    private ProgressBar bar;
    private Label label;

    [Export]
    private CustomTimer timer;
    public override void _Ready()
    {
        bar = GetNode<ProgressBar>("Bar");
        label = GetNode<Label>("Label");
        timer.Timeout += Stop;
        timer.Started += StartTimer;
        SetProcess(false);
    }
    
    public void Stop()
    {
        timer.Stop();
        SetProcess(false);
    }
    public void StartTimer(double length)
    {
        timer.Start(length);
        bar.MaxValue = length;
        SetProcess(true);
    }

    public override void _Process(double delta)
    {
        label.Text = timer.TimeLeft.ToString("F1");
        bar.Value = timer.TimeLeft;
    }
}
