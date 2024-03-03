public partial class SkillFirstStrike: BattleSkill
{
    private bool first;
    public SkillFirstStrike(): base(
        SkillId.FirstStrike,
        "T_SKL_FIRSTSTRIKE_TITLE",
        SkillType.OFFENSIVE,
        SkillElement.BLUNT,
        TargettingType.ENEMY_TARGET,
        false,
        3,
        2,
        false
    ) {}

    protected override void Execute(BattleEngine battleEngine, IBattleActor user, IBattleActor target, float targetEfficiency)
    {
        int damage;
        if (first) {
            damage = Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), targetEfficiency, 2);
            first = false;
        }
        else
            damage = Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), targetEfficiency, 1.5f);

        battleEngine.DoDamage(damage, user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, IBattleActor user, IBattleActor target)
    {
        if (first)
            return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), 1, 2.0f);
        else
            return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), 1, 1.5f);
    }

    public override string Description()
    {
        return "T_SKL_FIRSTSTRIKE_DESC";
    }
}