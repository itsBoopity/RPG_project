using Godot;
using System;

public class MonsterRack : Node2D
{
    private float width = 0;
    private float mouseSensitivity;
    private float mouseSmoothness;
    private float originalX;
    private float center;

    private Tween tween;
    private float targetX;

    public override void _Ready()
    {
        mouseSensitivity = Global.settings.monsterScrollSensitivity;
        mouseSmoothness = Global.settings.monsterScrollSmoothness;
        originalX = this.Position.x;
        targetX = originalX;
        center = originalX;
        tween = GetNode<Tween>("Tween"); 
    }
    public void Clear()
    {
        width = 0;
        this.Position = new Vector2(originalX, this.Position.y);
        foreach (Node node in GetChildren())
        {
            if (node.Name != "Tween") RemoveChild(node);
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_monsterScrollRight"))
        {
            if (targetX < originalX)
            {
                if (targetX < this.Position.x) targetX = this.Position.x;
                targetX += mouseSensitivity;

                tween.Stop(this);
                tween.InterpolateProperty(this, "position:x", this.Position.x, targetX, mouseSmoothness);
                tween.Start();
            }
        }
        else if (@event.IsActionPressed("battle_monsterScrollLeft"))
        {
            if (targetX > originalX - width)
            {
                if (targetX > this.Position.x) targetX = this.Position.x;
                targetX -= mouseSensitivity;

                tween.Stop(this);
                tween.InterpolateProperty(this, "position:x", this.Position.x, targetX, mouseSmoothness);
                tween.Start();
            }
        }
        else if (@event.IsActionPressed("battle_monsterScrollCenter"))
        {
            tween.Stop(this);
            targetX = center;
            this.Position = new Vector2(center, this.Position.y);
        }
    }
    public void AddModel(MonsterModel model)
    {   
        AddChild(model);
        model.Position = new Vector2(width, 0);
        width += model.GetBoundary().x;
        this.Position = new Vector2(Utility.MiddleX() - width/2, this.Position.y);
        center = this.Position.x;
        targetX = center;
    }

    public void Remove(MonsterModel model)
    {
        foreach(Node i in GetChildren())
        if (i == model) { RemoveChild(i); return;}
    }
}
