using Godot;

/// <summary>
/// Displays a message on screen that slowly moves upward and then fades out.
/// </summary>
public partial class FadingMessage : Node
{
    private AnimationPlayer animationPlayer;
    private Label label;

    public override void _Ready()
    {
        label = GetNode<Label>("Label");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _ExitTree()
    {
        animationPlayer.Stop();
        label.Text = "";
    }

    /// <summary>
    /// Shows message that then fades out.
    /// </summary>
    /// <param name="text"></param>
    public void ShowText(string text)
    {
        label.Text = text;
        animationPlayer.Stop();
        animationPlayer.Play("ShowText");
    }
}
