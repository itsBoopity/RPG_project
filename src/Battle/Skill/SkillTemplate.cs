using Godot;

[System.Serializable]
public partial class SkillTemplate: BattleSkill
{
    public SkillTemplate(): base("s_null")
    {
        name = "First Strike";
        type = SkillType.BASIC;
        element = SkillElement.NONE;
        targetting = TargettingType.NONE;
        cost = 0;
        cooldown = 0;
    }
    protected override void Execute(BattleEngine battleEngine, BattleActor user, BattleActor target, float targetEfficiency)
    {
    
    }
    public override int EstimateDamage(BattleEngine battleEngine, BattleActor user, BattleActor target)
    {
        return Utility.BasicDamageFormula(user.GetAtk(), target.GetDef());
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