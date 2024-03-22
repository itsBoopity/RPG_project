using Godot;

public partial class AnimatedSpriteOneOff : AnimatedSprite2D
{
    [Export]
    public AudioStreamWav[] sfxSrc;


    /// <summary>
    /// Percentual range of how much the pitch can deviate. i.e for 0.2, the pitch range from 90-110% shift.
    /// </summary>
    [Export]
    public float pitchRange = 0.2f;

    /// <summary>
    /// If false, plays at the location of attack on screen. Otherwise play regardless of position.
    /// </summary>
    [Export]
    public bool fullscreenEffect = false;

    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("Audio").Stream = sfxSrc[GD.Randi() % sfxSrc.Length];
        GetNode<AudioStreamPlayer>("Audio").PitchScale = (1.0f - pitchRange/2.0f) + GD.Randf() * pitchRange;
        GetNode<AudioStreamPlayer>("Audio").Play();
        Play();
    }

    public override void _ExitTree()
    {
        QueueFree();
    }

    public void DeleteMe()
    {
        EmitSignal(SignalName.AnimationFinished);
        QueueFree();
    }
}
