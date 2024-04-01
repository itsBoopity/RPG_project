using Godot;

public partial class AnimatedSpriteOneOff : Node2D
{
    [Signal]
    public delegate void AnimationFinishedEventHandler();

    /// <summary>
    /// If false, plays at the location of attack on screen. Otherwise play regardless of position.
    /// </summary>
    [Export]
    public bool fullscreenEffect = false;

    public void DeleteMe()
    {
        EmitSignal(SignalName.AnimationFinished);
        QueueFree();
    }

    public void AnimationPlayerFinished(string animName)
    {
        DeleteMe();
    }
}
