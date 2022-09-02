using Godot;
using System;

public class CustomizationTab : Tab
{
    Node windows;
    CustomizationColorTab colors;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        windows = GetNode("../../Windows");
        colors = GetNode<CustomizationColorTab>("../../Color");
    }

    public void TabBody()
    {
        if (windows.GetNode<CustomizationWindow>("Body").Visible) return;
        HideAll();
        windows.GetNode<CustomizationWindow>("Body").ShowAnimate();
        windows.GetNode<CustomizationWindow>("Head").ShowAnimate();
        colors.Visible = true;
        colors.SetTab(PlayerEnum.Skin);
    }

    public void TabFace()
    {
        if (windows.GetNode<CustomizationWindow>("Eye").Visible) return;
        HideAll();
        windows.GetNode<CustomizationWindow>("Brow").ShowAnimate();
        windows.GetNode<CustomizationWindow>("Eye").ShowAnimate();
        windows.GetNode<CustomizationWindow>("Nose").ShowAnimate();
        colors.Visible = true;
        colors.SetTab(PlayerEnum.Face);
    }

    public void TabHair()
    {
        if (windows.GetNode<CustomizationWindow>("Hair").Visible) return;
        HideAll();
        windows.GetNode<CustomizationWindow>("Hair").ShowAnimate();
        windows.GetNode<CustomizationWindow>("Sideburn").ShowAnimate();
        colors.Visible = true;
        colors.SetTab(PlayerEnum.HairColor);
    }

    public void TabBeard()
    {
        if (windows.GetNode<CustomizationWindow>("Beard").Visible) return;
        HideAll();
        windows.GetNode<CustomizationWindow>("Beard").ShowAnimate();
        windows.GetNode<CustomizationWindow>("Moustache").ShowAnimate();
        colors.Visible = true;
        colors.SetTab(PlayerEnum.BeardColor);
    }

    public void TabScruff()
    {
        if (windows.GetNode<CustomizationWindow>("Scruff").Visible) return;
        HideAll();
        windows.GetNode<CustomizationWindow>("Scruff").ShowAnimate();
        colors.Visible = true;
        colors.SetTab(PlayerEnum.Scruff);
    }

    public void TabColor()
    {
        if (windows.GetNode<CanvasItem>("Colors").Visible) return;
        HideAll();
        windows.GetNode<CustomizationColorWindow>("Colors").ShowAnimate();
    }

    private void HideAll()
    {
        foreach (Node i in windows.GetChildren())
        {
            ((CanvasItem) i).Visible = false;
        }
        colors.Visible = false;
        GetParent<TabCollection>().ClickTab(this);
    }
}
