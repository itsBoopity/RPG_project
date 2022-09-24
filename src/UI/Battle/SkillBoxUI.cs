using Godot;
using System;

public class SkillBoxUI : Sprite
{
    // The index of the skill that this skillbox represents;
    [Export] public int index;
    private Sprite fill;
    private Sprite icon;
    private Label stackCost;
    private Label cooldown;
    private CanvasItem snap;
    private Label cooldownCountdown;

    private Vector2 originalScale;
    [Export] private NodePath root = null;
    private BattleEngine battleEngine;
    private Color fadeColor = new Color(0.4f, 0.4f, 0.4f, 1);
    
    public override void _Ready()
    {
        fill = GetNode<Sprite>("Fill");
        icon = GetNode<Sprite>("Fill/Icon");
        stackCost = GetNode<Label>("Cost");
        cooldown = GetNode<Label>("Cooldown");
        snap = GetNode<CanvasItem>("SnapIcon");
        cooldownCountdown = GetNode<Label>("Countdown");


        originalScale = this.Scale;
        if (root == null) throw new Exception("SkillBoxUI does not have [Exoirt] root set");
        battleEngine = GetNode<BattleEngine>(root);

        GetNode<Label>("Hotkey").Text = ((InputEvent)InputMap.GetActionList("battle_skill" + index)[0]).AsText();
    }

    public void OnHover()
    {
        this.Scale = originalScale * 1.1f;
        battleEngine.ShowSkillDetail(index);
    }
    public void ExitHover()
    {
        this.Scale = originalScale;
        battleEngine.HideSkillDetail();
    }
    public void Grow() { this.Scale = originalScale * 1.1f; }
    public void Shrink() { this.Scale = originalScale; }
    public void OnPress() { battleEngine.SelectSkill(index); }

    public void Initiate(BattleSkill skill)
    {
        if (skill == null)
        {
            Empty();
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
            snap.Hide();
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

    public void Update(Character owner, BattleSkill skill)
    {
        Initiate(skill); // Only necessary if you plan to use the same skillbox for multiple characters/swapping out skillsZ

        int result = skill.IsUsable(owner);
        if (result ==  1)
        {
            fill.Modulate = fadeColor;
            cooldownCountdown.Show();
            cooldownCountdown.Text = skill.currentCooldown.ToString();
        }
        else if (result == 2 || result == 3)
        {
            fill.Modulate = fadeColor;
        }
        else
        {
            fill.Modulate = Colors.White;
            cooldownCountdown.Hide();
        }
    }

    public void Empty()
    {
        fill.Hide();
        stackCost.Hide();
        cooldown.Hide();
        cooldownCountdown.Hide();
        snap.Hide();
    }
}
