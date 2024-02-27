using Godot;
using System;

public partial class SkillDescription : NinePatchRect
{
    private RichTextLabel name;
    private RichTextLabel type;
    private RichTextLabel cost;
    private RichTextLabel cooldown;   
    private RichTextLabel description;

    public int currentFocus = -1; // -1 means no focus
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        name = GetNode<RichTextLabel>("Name");
        type = GetNode<RichTextLabel>("PropertyType");
        cost = GetNode<RichTextLabel>("PropertyCost");
        cooldown = GetNode<RichTextLabel>("PropertyCD");
        description = GetNode<RichTextLabel>("Description");

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        this.Hide();
    }

    public void ShowSkill(BattleSkill skill, int index)
    {
        currentFocus = index;

        name.Text = skill.name;
        type.Text = Utility.SkillBBName(skill.type);
        if (skill.type != SkillType.BASIC)
        {
            cost.Text = "| [st]Cost[/st] " + skill.cost;
            cooldown.Text = "| CD " + skill.cooldown;
        }
        else
        {
            cost.Text = "";
            cooldown.Text = "";
        }

        description.Text = "";
        if (Utility.IsPhysical(skill.element))
            description.Text += "- Type: "  + Utility.SkillBBName(skill.element) + "/Physical\n";
        else if (Utility.IsMagical(skill.element))
            description.Text += "- Type: "  + Utility.SkillBBName(skill.element) + "/Magical\n";
        
        if (!skill.isAoE) description.Text += "- Target: Single\n\n";
        else description.Text += "- Target: Multiple\n\n";

        description.Text += skill.Description();

        
        animationPlayer.Stop();
        animationPlayer.Play("PopIn");
    }

    public void HideSkill()
    {
        currentFocus = -1;
        this.Hide();
    }
}
