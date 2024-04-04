using Godot;

public partial class SidebarButton : Button
{
    Vector2 originalScale;
    
    public override void _Ready()
    {
        originalScale = Scale;
    }

    public void OnFocus()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Highlight");
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
    }

    public void OnUnfocus()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Unhighlight");
    }

    public void OnHover()
    {
        if (!HasFocus())
            GrabFocus();
    }
    public void OnUnhover()
    {
        if (HasFocus())
            ReleaseFocus();
    }



    public void ScaleDown()
    {
        this.Scale = new Vector2(originalScale.X * 0.97f , originalScale.Y);
    }

    public void ScaleBack()
    {
        this.Scale = originalScale;
    }
}
