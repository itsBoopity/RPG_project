using Godot;

public partial class MonsterAppendage : Sprite2D
{
    [Signal]
    public delegate void HitEventHandler(MonsterAppendage where);

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_clickTarget"))
        {
            if (IsPixelOpaque(GetLocalMousePosition()))
            {
                GetViewport().SetInputAsHandled();
                EmitSignal(SignalName.Hit, this);
            }
        }
    }
}
