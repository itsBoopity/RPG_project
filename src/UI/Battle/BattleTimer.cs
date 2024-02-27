using Godot;
using System;

public partial class BattleTimer : Sprite2D
{
    private ProgressBar bar;
    private Label label;
    private Timer timer;
    public override void _Ready()
    {
        bar = GetNode<ProgressBar>("Bar");
        label = GetNode<Label>("Label");
        timer = GetNode<Timer>("Timer");

        SetProcess(false);
    }
    
    public void Stop()
    {
        timer.Stop();
        SetProcess(false);
    }
    public void StartTimer(int length)
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
