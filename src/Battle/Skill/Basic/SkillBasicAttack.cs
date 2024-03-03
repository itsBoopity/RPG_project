public partial class SkillBasicAttack: BattleSkill
{
    public SkillBasicAttack(): base(
        SkillId.BasicAttack,
        "T_SKL_BASICATTACK_T",
        SkillType.BASIC,
        SkillElement.VARIABLE,
        TargettingType.ENEMY_TARGET,
        false,
        0,
        0,
        false
    ) {}

    protected override void Execute(BattleEngine battleEngine, BattleActor user, BattleActor target, float targetEfficiency)
    {
        user.stack += 1;
        battleEngine.DoDamage(Utility.BasicDamageFormula(user.GetAtk(), target.GetDef(), targetEfficiency), user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, BattleActor user, BattleActor target)
    {
        return Utility.BasicDamageFormula(user.GetAtk(), target.GetDef());
    }

    public override string Description()
    {
        return "T_SKL_BASICATTACK_DESC";
    }
}