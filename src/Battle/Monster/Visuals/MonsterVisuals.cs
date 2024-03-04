using Godot;

public partial class MonsterVisuals : Node
{
    private DamageEstimate dmgEstimate;
    private DamageCounter dmgCounter;
    private AnimationPlayer animationPlayer;

    private TextureProgressBar hpBar;
    private Label hpCurrent;
    private Label hpMax;

    public Node GetAnimator() { return animationPlayer; }

    public override void _Ready()
    {
        dmgEstimate = GetNode<DamageEstimate>("UI/DamageEstimate");
        dmgCounter = GetNode<DamageCounter>("UI/DamageCounter");
        hpCurrent = GetNode<Label>("UI/HP/Current");
        hpMax = GetNode<Label>("UI/HP/Max");
        hpBar = GetNode<TextureProgressBar>("UI/HP/Bar");

        animationPlayer = GetNode<AnimationPlayer>("Sprite/AnimationPlayer");
    }

    public Vector2 GetBoundary()
    {
        return GetNode<Control>("BoundaryBox").Size;
    }

    public void UpdateHP(int health, int maxHealth)
    {
        hpCurrent.Text = health.ToString();
        hpMax.Text = maxHealth.ToString();
        hpBar.MaxValue = maxHealth;
        if (hpBar.Value != health)
        {
            Tween tween = CreateTween();
            tween.TweenProperty(hpBar, "value", health, 0.2f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Elastic);
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
        dmgCounter.Play(damage);
    }
    public void DisplayText(string text)
    {
        HideEstimate();
        dmgCounter.Play(text);
    }

    public void PlayMiss()
    {
        DisplayText("Miss");
    }

    public void Animate(string name)
    {
        animationPlayer.Stop();
        animationPlayer.Play(name);
    }

    /// <summary>
    /// Plays the animation for the monster being destroyed.
    /// </summary>
    /// <returns>The AnimationPlayer running the animation.</returns>
    public AnimationPlayer AnimateDefeat()
    {
        GetNode<AnimationPlayer>("Sprite/BaseAnimations").Play("Death");
        return GetNode<AnimationPlayer>("Sprite/BaseAnimations");
    }
}