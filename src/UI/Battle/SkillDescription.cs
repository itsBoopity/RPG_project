using Godot;

public partial class SkillDescription : NinePatchRect
{
    private RichTextLabel name;
    private RichTextLabel type;
    private RichTextLabel cost;
    private RichTextLabel cooldown;
    private RichTextLabel description;

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
        if (Utility.IsPhysical(skill.Element))
            description.Text += $"- {Tr("T_B_SKILLTYPE")}: {Utility.Instance.SkillBBName(skill.Element)}/{Tr("T_SKT_PHYSICAL")}\n";
        else if (Utility.IsMagical(skill.Element))
            description.Text += $"- {Tr("T_B_SKILLTYPE")}: {Utility.Instance.SkillBBName(skill.Element)}/{Tr("T_SKT_MAGICAL")}\n";
        
        if (!skill.IsAoE) description.Text += "- Target: Single\n\n";
        else description.Text += "- Target: Multiple\n\n";

        description.Text += Tr(skill.Description);

        
        animationPlayer.Stop();
        animationPlayer.Play("PopIn");
    }

    public void HideSkill()
    {
        this.Hide();
    }
}
