using Godot;

using System.Collections.Generic;
using System.Linq;

public partial class MonsterRack : Node2D
{
    private float width = 0;
    private float mouseSensitivity;
    private float mouseSmoothness;
    private float keyHoldSpeed;
    private float center;

    private Tween tween;
    private float targetX;

    public int Count
    {
        get
        {
            int output = 0;
            foreach (BattleMonster child in GetAll())
            {
                if (!child.IsDisappearing)
                {
                    output += 1;
                }
            }
            return output;
        }
    }

    public override void _Ready()
    {
        mouseSensitivity = Global.Settings.monsterScrollSensitivity;
        mouseSmoothness = Global.Settings.monsterScrollSmoothness;
        keyHoldSpeed = Global.Settings.monsterScrollSpeed;
        center = this.Position.X;
        targetX = center;
        tween = CreateTween();
        tween.Stop();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
            tween?.Kill();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("battle_monsterScrollLeft"))
        {
            targetX -= mouseSensitivity;
            if (targetX < center - width/2)
            {
                targetX = center - width/2;
            }
            tween.Kill();
            tween = CreateTween();
            tween.TweenProperty(this, "position:x", targetX, mouseSmoothness);
        }
        else if (@event.IsActionPressed("battle_monsterScrollRight"))
        {
            targetX += mouseSensitivity;
            if (targetX > center + width/2)
            {
                targetX = center + width/2;
            }
            tween.Kill();
            tween = CreateTween();
            tween.TweenProperty(this, "position:x", targetX, mouseSmoothness);
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
            if (this.Position.X < center - width/2)
            {
                this.Position = new Vector2(center - width/2, this.Position.Y);
            }
            else if (this.Position.X > center + width/2)
            {
                this.Position = new Vector2(center + width/2, this.Position.Y);
            }
            targetX = this.Position.X;
        }
    }

    public void Add(BattleMonster monster)
    {   
        AddChild(monster);
        monster.Position = new Vector2(width, 0);
        width += monster.GetBoundary().X;
        this.Position = new Vector2(-width/2, this.Position.Y);
        center = this.Position.X;
        targetX = center;
    }

    public void Clear()
    {
        width = 0;
        this.Position = new Vector2(0, this.Position.Y);
        foreach (Node node in GetChildren())
        {
            if (node.Name != "Tween") node.Free();
        }
    }

    public IEnumerable<BattleMonster> GetAll()
	{
		return GetChildren().Cast<BattleMonster>();
	}

    public void HideEstimateAll()
    {
        foreach (BattleMonster monster in GetChildren().Cast<BattleMonster>())
        {
            monster.HideEstimate();
        }
    }
}

