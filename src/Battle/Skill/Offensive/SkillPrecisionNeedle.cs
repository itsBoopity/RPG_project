public partial class SkillPrecisionNeedle: BattleSkill
{
    public SkillPrecisionNeedle(): base(
        SkillId.PrecisionNeedle,
        "T_SKL_PRECISIONNEEDLE_TITLE",
        SkillType.OFFENSIVE,
        SkillElement.PIERCE,
        TargettingType.ENEMY_TARGET,
        false,
        2,
        2,
        false
    ) {}
    protected override void Execute(BattleEngine battleEngine, IBattleActor user, IBattleActor target, float targetEfficiency)
    {
        int damage = 1;
        
        if (targetEfficiency >= 1.2f) damage = 5;
        else if (targetEfficiency >= 0.90f) damage = 3;
        else if (targetEfficiency >= 0.6f) damage = 2;

        battleEngine.DoDamage(damage, user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, IBattleActor user, IBattleActor target)
    {
        return -1;
    }

    public override string Description()
    {
        return "T_SKL_PRECISIONNEEDLE_DESC";
    }
}