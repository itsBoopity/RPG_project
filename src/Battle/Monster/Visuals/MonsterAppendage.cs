using Godot;

public partial class MonsterAppendage : Sprite2D
{
    [Signal]
    public delegate void HitEventHandler(MonsterAppendage where);

    [Export]
    public bool targettable = true;

    [Export]
    public int appendageId = 0;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_clickTarget"))
        {
            if (targettable && IsPixelOpaque(GetLocalMousePosition()))
            {
                GetViewport().SetInputAsHandled();
                EmitSignal(SignalName.Hit, this);
            }
        }
    }
}
