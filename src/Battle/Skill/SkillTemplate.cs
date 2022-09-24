using Godot;

[System.Serializable]
public class SkillTemplate: BattleSkill
{
    public SkillTemplate()
    {
        name = "First Strike";
        type = SkillType.BASIC;
        element = SkillElement.NONE;
        targetting = TargettingType.NONE;
        cost = 0;
        cooldown = 0;
    }
    protected override void Execute(BattleEngine battleEngine, BattleFigure user, BattleFigure target, float targetEfficiency)
    {
    
    }
    public override int EstimateDamage(BattleEngine battleEngine, BattleFigure user, BattleFigure target)
    {
        return Utility.BasicDamageFormula(user.GetATK(), target.GetDEF());
    }
    public override string Description()
    {
        return "";
    }

    // Overwrite if the skill needs to reset internal parameters
    // public override void Reset()
    // {
    //     base.Reset();
    //     // Rest of the code
    // }
}