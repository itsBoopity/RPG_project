using Godot;
using System;

public class MonsterTarget : Sprite
{
    [Signal] delegate void Hit(MonsterTarget target);
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_clickTarget"))
        {
            if (this.IsPixelOpaque(GetLocalMousePosition()))
            {
                EmitSignal("Hit", this);
                GetTree().SetInputAsHandled();
            }
        }
    }
}
