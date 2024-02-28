using Godot;

public partial class SquareButton : TextureButton
{
    private Vector2 originalSize;
    private float expandFactor = 1.15f;
    private float midExpandFactor = 1.07f;
    public override void _Ready()
    {
        //selector = GetNode<Selector>("../../Selector");
        originalSize = Scale;
    }

    public void ScaleBack()
    {
        Scale = originalSize;
    }

    public void ScaleMid()
    {
        Scale = originalSize * midExpandFactor;
    }

    public void ScaleUp()
    {
        Scale = originalSize * expandFactor;
    }

    public virtual void OnPress() {}
}
