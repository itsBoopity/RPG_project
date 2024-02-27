using Godot;

public partial class Reticle : Node2D
{
    public override void _Ready()
    {
        Disable();
    }
    public override void _Process(double delta)
    {
        this.Position = GetGlobalMousePosition();
    }

    public void ShowReticle()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        this.Position = GetGlobalMousePosition();
        this.SetProcess(true);
        this.Show();
    }

    public void Disable()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        this.SetProcess(false);
        this.Hide();
    }
}
