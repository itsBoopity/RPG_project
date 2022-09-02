using Godot;

public class SkillTemplate: BattleSkill
{
    public SkillTemplate()
    {
        name = "SkillTemplate";
        type = SkillType.BASIC;
        cost = 0;
        cooldown = 0;
        textureX = 0;
        textureY = 0;
    }
    public override void Execute(BattleFigure user, BattleFigure target)
    {

    }
    public override string Description()
    {
        return "";
    }
}