using Godot;
using System;

public partial class ControlBoundaryTest : Button
{

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouse)
        {
            string text = "Position: " + GetRect().Position.ToString() + "\n";
			text += "Size: " + GetRect().Size.ToString() + "\n";
			text += "LocMou: " + GetLocalMousePosition() + "\n";
			text += "EvtMou: " + mouse.Position.ToString()+ "\n";
			text += GetRect().HasPoint(mouse.Position)+ "\n";
			this.Text = text;
        }
    }
}
