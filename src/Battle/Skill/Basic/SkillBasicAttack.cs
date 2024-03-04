public partial class SkillBasicAttack: BattleSkill
{
    public SkillBasicAttack(): base(
        SkillId.BasicAttack,
        "T_SKL_BASICATTACK_TITLE",
        SkillType.BASIC,
        SkillElement.VARIABLE,
        TargettingType.ENEMY_TARGET,
        false,
        0,
        0,
        false
    ) {}

    protected override void Execute(BattleEngine battleEngine, IBattleActor user, IBattleActor target, float appendageCoef)
    {
        user.ChangeStack(1);
        battleEngine.DoDamage(Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), appendageCoef), user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, IBattleActor user, IBattleActor target)
    {
        return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense());
    }

    public override string Description()
    {
        return "T_SKL_BASICATTACK_DESC";
    }
}