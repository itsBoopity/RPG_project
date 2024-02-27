using Godot;

public partial class MonsterTarget : Sprite2D
{
    [Signal]
    public delegate void TargetHitEventHandler(MonsterTarget where);
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_clickTarget"))
        {
            if (IsPixelOpaque(GetLocalMousePosition()))
            {
                GetViewport().SetInputAsHandled();
                EmitSignal(SignalName.TargetHit, this);
            }
        }
    }
}
