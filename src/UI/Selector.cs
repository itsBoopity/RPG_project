using Godot;

public partial class Selector : Node2D
{
    private BaseButton following;
    private Vector2 originalScale;

    public override void _Ready()
    {
        originalScale = Scale;
    }

    public override void _Process(double delta)
    {
        if (following != null)
        {
            this.GlobalPosition = following.GlobalPosition;
            this.Scale = originalScale * following.Scale.X;
        }
    }

    public void Select(BaseButton button)
    {
        following = button;
    }
}
