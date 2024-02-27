using Godot;
using System;

public partial class MonsterRack : Node2D
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
        mouseSensitivity = Global.Settings.monsterScrollSensitivity;
        mouseSmoothness = Global.Settings.monsterScrollSmoothness;
        originalX = this.Position.X;
        targetX = originalX;
        center = originalX;
        tween = CreateTween();
        tween.Stop();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
            if (tween != null) tween.Kill();
    }

    public void Clear()
    {
        width = 0;
        this.Position = new Vector2(originalX, this.Position.Y);
        foreach (Node node in GetChildren())
        {
            if (node.Name != "Tween") node.QueueFree();
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_monsterScrollRight"))
        {
            if (targetX < originalX)
            {
                if (targetX < this.Position.X) targetX = this.Position.X;
                targetX += mouseSensitivity;

                tween.Kill();
                tween = CreateTween();
                tween.TweenProperty(this, "position:x", targetX, mouseSmoothness);
            }
        }
        else if (@event.IsActionPressed("battle_monsterScrollLeft"))
        {
            if (targetX > originalX - width)
            {
                if (targetX > this.Position.X) targetX = this.Position.X;
                targetX -= mouseSensitivity;

                tween.Kill();
                tween = CreateTween();
                tween.TweenProperty(this, "position:x", targetX, mouseSmoothness);
            }
        }
        else if (@event.IsActionPressed("battle_monsterScrollCenter"))
        {
            tween.Kill();
            targetX = center;
            this.Position = new Vector2(center, this.Position.Y);
        }
    }
    public void AddModel(MonsterModel model)
    {   
        AddChild(model);
        model.Position = new Vector2(width, 0);
        width += model.GetBoundary().X;
        this.Position = new Vector2(Utility.MiddleX() - width/2, this.Position.Y);
        center = this.Position.X;
        targetX = center;
    }
}
