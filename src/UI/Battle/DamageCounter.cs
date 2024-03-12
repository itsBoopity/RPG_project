using Godot;

public partial class DamageCounter : Node
{
    private AnimationPlayer animationPlayer;
    private Label number;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        number = GetNode<Label>("Number");
    }

    public void Play(int damage)
    {
        number.Text = damage.ToString();
        animationPlayer.Stop();
        animationPlayer.Play("Shake");
    }

    public void Play(string text)
    {
        number.Text = text;
        animationPlayer.Stop();
        animationPlayer.Play("Shake");
    }
}
