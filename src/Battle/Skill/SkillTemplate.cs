public partial class SkillTemplate: BattleSkill
{
    public SkillTemplate(): base(
        SkillId.Null, "SKILL_NULL", 0, 0, 0, false, 0, 0, false
    ) {}
    protected override void Execute(BattleEngine battleEngine, IBattleActor user, IBattleActor target, float appendageCoef)
    {
    
    }
    public override int EstimateDamage(BattleEngine battleEngine, IBattleActor user, IBattleActor target)
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