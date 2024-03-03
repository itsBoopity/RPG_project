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

    protected override void Execute(BattleEngine battleEngine, BattleActor user, BattleActor target, float targetEfficiency)
    {
        int damage;
        if (first) {
            damage = Utility.BasicDamageFormula(user.GetAtk(), target.GetDef(), targetEfficiency, 2);
            first = false;
        }
        else
            damage = Utility.BasicDamageFormula(user.GetAtk(), target.GetDef(), targetEfficiency, 1.5f);

        battleEngine.DoDamage(damage, user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, BattleActor user, BattleActor target)
    {
        if (first)
            return Utility.BasicDamageFormula(user.GetAtk(), target.GetDef(), 1, 2);
        else
            return Utility.BasicDamageFormula(user.GetAtk(), target.GetDef(), 1, 1.5f);
    }

    public override string Description()
    {
        return "T_SKL_FIRSTSTRIKE_DESC";
        return "- Do " + Utility.ATK("2*ATK") + " damage on first use\n- Do " + Utility.ATK("1.5*ATK") + " the rest of the combat";
    }
}