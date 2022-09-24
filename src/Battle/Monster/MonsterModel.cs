using Godot;
using System;

public class MonsterModel : Node2D
{
    private DamageCounter counter;
    private Monster owner = null;
    private AnimationPlayer animationPlayer;
    private Control boundary;

    private Label hpCurrent;
    private Label hpMax;

    public override void _Ready()
    {
        counter = GetNode<DamageCounter>("DamageCounter");
        hpCurrent = GetNode<Label>("HP/HPCurrent");
        hpMax = GetNode<Label>("HP/HPMax");

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        boundary = GetNode<Control>("BoundaryBox");
    }

    public void SetOwner(Monster owner) { this.owner = owner; }

    public Vector2 GetBoundary()
    {
        if (boundary == null) throw new Exception("MonsterModel.GetBoundary called before model got added to the scenetree");
        return boundary.RectSize;
    }

    public void UpdateHP()
    {
        if (owner != null)
        {
            hpCurrent.Text = owner.HP.ToString();
            hpMax.Text = owner.maxHP.ToString();
        }
    }

    public void ShowEstimate(int damage) { counter.ShowEstimate(damage); }
    public void HideEstimate() { counter.HideEstimate(); }
    public void PlayDamage(int damage) { counter.Play(damage); }
    public void PlayDamage(string text) { counter.Play(text); }

    public void TargetMiss() { if (owner != null) owner.TargetMiss(); }

    public void TargetHit(MonsterTarget target) { if (owner != null) owner.Hit(target);  }

    public void Animate(string name)
    {
        animationPlayer.Stop();
        animationPlayer.Play(name);
    }
}
