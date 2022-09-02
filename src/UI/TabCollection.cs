using Godot;
using System;

public class TabCollection : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ClickTab(GetChild<Tab>(0));
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void ClickTab(Tab tab)
    {
        foreach (Node i in GetChildren())
        {
            ((CanvasItem)i).SelfModulate = new Color(1, 1, 1);
        }
        tab.SelfModulate = new Color(1, 0.835f, 0.47f);
    }
}
