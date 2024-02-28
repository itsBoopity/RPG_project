using Godot;

public partial class TabCollection : Control
{
    public override void _Ready()
    {
        ClickTab(GetChild<Tab>(0));
    }

    public void ClickTab(Tab tab)
    {
        foreach (Node i in GetChildren())
        {
            ((CanvasItem)i).SelfModulate = new Color(1, 1, 1);
        }
        tab.SelfModulate = new Color(1, 0.835f, 0.47f);
    }
}
