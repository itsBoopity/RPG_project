using Godot;
using System;

public class MonsterTarget : Sprite
{
    [Export] private NodePath rootPath = null;
    private MonsterModel root;

    public override void _Ready()
    {
        root = GetNode<MonsterModel>(rootPath);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_clickTarget"))
        {
            if (this.IsPixelOpaque(GetLocalMousePosition()))
            {
                GetTree().SetInputAsHandled();
                root.TargetHit(this);
            }
        }
    }
}
