using Godot;

public partial class TalkAnimator : AnimationPlayer
{
    [Export] private float variation = 0.5f;
    public void RandomSpeed()
    {
        this.SpeedScale = 1f + GD.Randf() * variation;
    }
}
