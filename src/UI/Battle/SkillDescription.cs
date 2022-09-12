using Godot;
using System;

public class SkillDescription : NinePatchRect
{
    private RichTextLabel name;
    private RichTextLabel type;
    private RichTextLabel cost;
    private RichTextLabel cooldown;   
    private RichTextLabel description;

    public int currentFocus = 0;
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

        name.BbcodeText = skill.name;
        type.BbcodeText = Utility.SkillBBName(skill.type);
        if (skill.type != SkillType.BASIC)
        {
            cost.BbcodeText = "| [st]Cost[/st] " + skill.cost;
            cooldown.BbcodeText = "| CD " + skill.cooldown;
        }
        else
        {
            cost.BbcodeText = "";
            cooldown.BbcodeText = "";
        }

        description.BbcodeText = "";
        if (Utility.IsPhysical(skill.element))
            description.BbcodeText += "- Type: "  + Utility.SkillBBName(skill.element) + "/Physical\n";
        else if (Utility.IsMagical(skill.element))
            description.BbcodeText += "- Type: "  + Utility.SkillBBName(skill.element) + "/Magical\n";
        
        if (!skill.isAoE) description.BbcodeText += "- Target: Single\n\n";
        else description.BbcodeText += "- Target: Multiple\n\n";

        description.BbcodeText += skill.Description();

        
        animationPlayer.Stop();
        animationPlayer.Play("PopIn");
    }
}
