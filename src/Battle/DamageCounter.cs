using Godot;

public partial class DamageCounter : Node
{
    private AnimationPlayer animationPlayer;
    private Label estimate;
    private Label number;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        estimate = GetNodeOrNull<Label>("Label");
        number = GetNode<Label>("Number");
    }

    public void Play(int damage)
    {
        if (estimate != null) HideEstimate();
        number.Text = damage.ToString();
        animationPlayer.Stop();
        animationPlayer.Play("Shake");
    }

    public void Play(string text)
    {
        if (estimate != null) HideEstimate();
        number.Text = text;
        animationPlayer.Stop();
        animationPlayer.Play("Shake");
    }

    public void HideEstimate()
    {
        estimate.Hide();
        number.Hide();
    }

    public void ShowEstimate(int damage)
    {
        if (damage == -1)
            number.Text = "?";
        else    
            number.Text = damage.ToString();
        animationPlayer.Play("ViewEstimate");
    }
}
