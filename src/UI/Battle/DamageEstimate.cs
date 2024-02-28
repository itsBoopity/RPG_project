using Godot;

public partial class DamageEstimate : Control
{
    private Label number;

    public override void _Ready()
    {
        number = GetNode<Label>("Number");
    }
    public void HideEstimate()
    {
        this.Hide();
    }

    public void ShowEstimate(int damage)
    {
        if (damage == -1)
            number.Text = "?";
        else    
            number.Text = damage.ToString();
        this.Show();
    }
}
