using Godot;
using System;

public partial class MonsterModel : CanvasGroup
{
    private Monster owner = null;
    private DamageEstimate dmgEstimate;
    private DamageCounter dmgCounter;
    private AnimationPlayer animationPlayer;
    private Control boundary;

    private TextureProgressBar hpBar;
    private Label hpCurrent;
    private Label hpMax;

    public Node GetAnimator() { return animationPlayer; }

    public override void _Ready()
    {
        dmgEstimate = GetNode<DamageEstimate>("OverheadUI/DamageEstimate");
        dmgCounter = GetNode<DamageCounter>("OverheadUI/DamageCounter");
        hpCurrent = GetNode<Label>("OverheadUI/HP/Current");
        hpMax = GetNode<Label>("OverheadUI/HP/Max");
        hpBar = GetNode<TextureProgressBar>("OverheadUI/HP/Bar");

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        boundary = GetNode<Control>("BoundaryBox");
    }

    public void SetOwner(Monster owner) { this.owner = owner; }

    public Vector2 GetBoundary()
    {
        if (boundary == null)
        {
            throw new Exception("MonsterModel.GetBoundary called before model got added to the scenetree");
        }
        return boundary.Size;
    }

    public void UpdateHP()
    {
        if (owner != null)
        {
            hpCurrent.Text = owner.hp.ToString();
            hpMax.Text = owner.maxHp.ToString();
            hpBar.MaxValue = owner.maxHp;
            if (hpBar.Value != owner.hp)
            {
                Tween tween = CreateTween();
                tween.TweenProperty(hpBar, "value", owner.hp, 0.2f).
                    SetEase(Tween.EaseType.InOut).
                    SetTrans(Tween.TransitionType.Elastic);
            }
        }
    }

    public void ShowEstimate(int damage)
    {
        dmgEstimate.ShowEstimate(damage);
    }
    public void HideEstimate()
    {
        dmgEstimate.HideEstimate();
    }
    public void PlayDamage(int damage)
    {
        HideEstimate();
        this.dmgCounter.Play(damage);
    }
    public void PlayDamage(string text)
    {
        HideEstimate();
        dmgCounter.Play(text);
    }

    public void TargetMiss()
    {
        owner?.TargetMiss(); 
    }

    public void TargetHit(MonsterTarget target)
    {
        owner?.Hit(target);
    }

    public void Animate(string name)
    {
        animationPlayer.Stop();
        animationPlayer.Play(name);
    }

    public void AnimateDefeat()
    {
        GetNode<AnimationPlayer>("BaseAnimations").Play("Death");
    }
}
