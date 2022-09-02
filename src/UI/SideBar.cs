using Godot;
using System;

public class SideBar : Sprite
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetChild<Control>(1).GetChild<Control>(0).GrabFocus();
        //if (nosavedata) then Disable continue
    }
}
