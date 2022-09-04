using Godot;

[System.Serializable]
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

    // Overwrite if the skill needs to reset internal parameters
    // public new void Reset()
    // {
    //     base.Reset();
    // }
}