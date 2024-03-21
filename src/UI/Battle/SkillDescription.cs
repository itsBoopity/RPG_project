using Godot;

public partial class SkillDescription : Control
{
    private RichTextLabel name;
    private RichTextLabel type;
    private RichTextLabel cost;
    private RichTextLabel cooldown;
    private RichTextLabel description;
    private AnimationPlayer animationPlayer;

    private NinePatchRect rect;
    public override void _Ready()
    {
        name = GetNode<RichTextLabel>("NinePatchRect/Name");
        type = GetNode<RichTextLabel>("NinePatchRect/PropertyType");
        cost = GetNode<RichTextLabel>("NinePatchRect/PropertyCost");
        cooldown = GetNode<RichTextLabel>("NinePatchRect/PropertyCD");
        description = GetNode<RichTextLabel>("NinePatchRect/Description");
        rect = GetNode<NinePatchRect>("NinePatchRect");
        rect.Hide();

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void ShowSkill(BattleSkill skill)
    {
        name.Text = skill.DisplayName;
        type.Text = Utility.SkillBBName(skill.Type);
        if (skill.Type != SkillType.BASIC)
        {
            cost.Text = $"| [st]{Tr("T_B_SKILLCOST")}[/st] " + skill.Cost;
            cooldown.Text = $"| {Tr("T_B_CD")} " + skill.Cooldown;
        }
        else
        {
            cost.Text = "";
            cooldown.Text = "";
        }

        description.Text = "";
        if (skill.Element.IsPhysical())
            description.Text += $"- {Tr("T_B_SKILLTYPE")}: {Utility.Instance.SkillBBName(skill.Element)}/{Tr("T_SKT_PHYSICAL")}\n";
        else if (skill.Element.IsMagical())
            description.Text += $"- {Tr("T_B_SKILLTYPE")}: {Utility.Instance.SkillBBName(skill.Element)}/{Tr("T_SKT_MAGICAL")}\n";
        
        if (!skill.IsAoE) description.Text += "- Target: Single\n\n";
        else description.Text += "- Target: Multiple\n\n";

        description.Text += Tr(skill.Description);

        Show();
        animationPlayer.Stop();
        animationPlayer.Play("PopIn");
    }

    public void HideSkill()
    {
        Hide();
    }

    public void OnHover()
    {
        rect.Hide();
    }

    public void ExitHover()
    {
        rect.Show();
    }
}
