using Godot;

public partial class StackSpriteVisuals : MonsterVisuals
{
	private AnimationPlayer orbAnimator; 
    private Timer orbTimer; 
    private bool orbRevealed = false;

    public override void _Ready()
    {
        base._Ready();
        orbAnimator = GetNode<AnimationPlayer>("%OrbPlayer");
        orbTimer = GetNode<Timer>("Timer");
        TimerDowntime();
    }

	private void TimerDowntime()
	{
		orbTimer.Start(10 + GD.Randi() % 20);
	}
	private void TimerReveal()
	{
		orbTimer.Start(2 + GD.Randf() * 3);
	}

	private void OrbTimeout()
	{
		if (orbRevealed)
		{
			orbAnimator.Play("HideOrb");
			TimerReveal();
		}
		else
		{
			orbAnimator.Play("ShowOrb");
			TimerDowntime();
		}
		orbRevealed = !orbRevealed;
	}
}
