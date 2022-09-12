using Godot;
using System;

public class SkillBoxUI : Sprite
{
    // The index of the skill that this skillbox represents;
    public int index;
    private Sprite fill;
    private Sprite icon;
    private Label stackCost;
    private Label cooldown;
    private CanvasItem snap;
    private Label cooldownCountdown;

    private Vector2 originalScale;
    [Signal] delegate void HoverSkill();
    [Signal] delegate void ExitHoverSkill();
    [Signal] delegate void SelectSkill();
    private Color fadeColor = new Color(0.4f, 0.4f, 0.4f, 1);
    
    public override void _Ready()
    {
        originalScale = this.Scale;
        fill = GetNode<Sprite>("Fill");
        icon = GetNode<Sprite>("Fill/Icon");
        stackCost = GetNode<Label>("Cost");
        cooldown = GetNode<Label>("Cooldown");
        snap = GetNode<CanvasItem>("SnapIcon");
        cooldownCountdown = GetNode<Label>("Countdown");

        GetNode<Label>("Hotkey").Text = ((InputEvent)InputMap.GetActionList("battle_skill" + Name)[0]).AsText();
    }

    public void OnHover()
    {
        this.Scale = originalScale * 1.1f;
        //EmitSignal("HoverSkill");
    }
    public void ExitHover()
    {
        this.Scale = originalScale;
        //EmitSignal("ExitHoverSkill");
    }
    public void Grow() { this.Scale = originalScale * 1.1f; }
    public void Shrink() { this.Scale = originalScale; }
    public void OnPress() { //EmitSignal("SelectSkill"); 
    }

    public void Initiate(BattleSkill skill)
    {
        if (skill == null)
        {
            fill.Hide();
            stackCost.Hide();
            cooldown.Hide();
            cooldownCountdown.Hide();
            snap.Hide();
            return;
        }
        else
        {
            fill.Show();
            stackCost.Show();
            cooldown.Show();
            cooldownCountdown.Hide();
        }
        
        icon.Texture = skill.GetIcon();

        fill.Texture = GD.Load<Texture>("res://Images/UI/Battle/Fill" + skill.type.ToString() + ".png");
        
        if (skill.type == SkillType.BASIC)
        {
            stackCost.Hide();
            cooldown.Hide();
        }
        else
        {
            stackCost.Text = skill.cost.ToString();
            cooldown.Text = skill.cooldown.ToString();
            if (skill.snap)
                snap.Show();
            else
                snap.Hide();
        }
    }

    public void Update(BattleSkill skill)
    {
        Initiate(skill); // Only necessary if you plan to use the same skillbox for multiple characters/swapping out skills
        if (skill == null)
         return;

        if (skill.currentCooldown > 0)
        {
            fill.Modulate = fadeColor;
            cooldownCountdown.Show();
            cooldownCountdown.Text = skill.currentCooldown.ToString();
        }
        else
        {
            fill.Modulate = Colors.White;
            cooldownCountdown.Hide();
        }
    }
}
