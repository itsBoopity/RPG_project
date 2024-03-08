public partial class SkillTemplate: BattleSkillData
{
    public SkillTemplate(): base(
        SkillId.Null, "SKILL_NULL", 0, 0, 0, false, 0, 0, false
    ) {}
    
    public override void Execute(BattleActor user, BattleActor target, float appendageCoef)
    {
    
    }
    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense());
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