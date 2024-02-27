using Godot;

public partial class MonsterRack : Node2D
{
    private float width = 0;
    private float mouseSensitivity;
    private float mouseSmoothness;
    private float keyHoldSpeed;
    private float originalX;
    private float center;

    private Tween tween;
    private float targetX;

    public override void _Ready()
    {
        mouseSensitivity = Global.Settings.monsterScrollSensitivity;
        mouseSmoothness = Global.Settings.monsterScrollSmoothness;
        keyHoldSpeed = Global.Settings.monsterScrollSpeed;
        originalX = this.Position.X;
        targetX = originalX;
        center = originalX;
        tween = CreateTween();
        tween.Stop();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
            tween?.Kill();
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
        if (@event.IsActionPressed("battle_monsterScrollLeft"))
        {
            targetX -= mouseSensitivity;
            if (targetX < originalX - width)
            {
                targetX = originalX - width;
            }
            tween.Kill();
            tween = CreateTween();
            tween.TweenProperty(this, "position:x", targetX, mouseSmoothness);
            
        }
        else if (@event.IsActionPressed("battle_monsterScrollRight"))
        {
            if (targetX < originalX)
            {
                targetX += mouseSensitivity;
                if (targetX > originalX)
                {
                    targetX = originalX;
                }
                tween.Kill();
                tween = CreateTween();
                tween.TweenProperty(this, "position:x", targetX, mouseSmoothness);
            }
        }
        else if (@event.IsActionPressed("battle_monsterHoldScrollLeft"))
        {
            tween.Kill();
            targetX = this.Position.X;
        }
        else if (@event.IsActionPressed("battle_monsterHoldScrollRight"))
        {
            tween.Kill();
            targetX = this.Position.X;
        }
        else if (@event.IsActionPressed("battle_monsterScrollCenter"))
        {
            tween.Kill();
            targetX = center;
            this.Position = new Vector2(center, this.Position.Y);
        }    
    }

    public override void _Process(double delta)
    {
        float direction = Input.GetAxis("battle_monsterHoldScrollLeft", "battle_monsterHoldScrollRight");
        if (direction != 0.0)
        {
            this.Translate(new Vector2(direction * (float)delta * keyHoldSpeed, 0));
            if (this.Position.X < originalX - width)
            {
                this.Position = new Vector2(originalX - width, this.Position.Y);
            }
            else if (this.Position.X > originalX)
            {
                this.Position = new Vector2(originalX, this.Position.Y);
            }
            targetX = this.Position.X;
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
