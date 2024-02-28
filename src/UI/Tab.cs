using Godot;
public partial class Tab : TextureButton
{
    private Vector2 originalScale;

    public override void _Ready()
    {
        originalScale = this.Scale;
    }

    public void PushUp()
    {
        this.Position -= new Vector2(0,10).Rotated(this.Rotation);
    }

    public void PushBack()
    {
        this.Position += new Vector2(0,10).Rotated(this.Rotation);
    }

    public void ScaleDown()
    {
        this.Scale = originalScale * 0.98f;
    }

    public void ScaleBack()
    {
        this.Scale = originalScale;
    }
}
