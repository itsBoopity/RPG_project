using Godot;
using System;

public partial class SidebarButton : Button
{
    SidebarHighlight highlight;
    Vector2 originalScale;
    
    public override void _Ready()
    {
        highlight = GetNode<SidebarHighlight>("../../SidebarHighlight");
        originalScale = Scale;
    }

    public void OnFocus()
    {
        this.GrabFocus();
        highlight.Focus(this);
    }

    public void OnHover()
    {
        if (!this.HasFocus())
            OnFocus();
    }

    public void ScaleDown()
    {
        this.Scale = new Vector2(originalScale.X * 0.97f , originalScale.Y);
        highlight.ScaleDown();
    }

    public void ScaleBack()
    {
        this.Scale = originalScale;
        highlight.ScaleBack();
    }
}
