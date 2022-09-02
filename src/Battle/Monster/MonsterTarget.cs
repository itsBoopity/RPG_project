using Godot;
using System;

public class MonsterTarget : Sprite
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_clickTarget"))
        {
            if (this.IsPixelOpaque(GetLocalMousePosition()))
            {
                //THIS IS WHERE THE HIT SIGNAL IS EMITTED
                GetTree().SetInputAsHandled();
            }
        }
    }
}
