public partial class SkillPrecisionNeedle: BattleSkill
{
    public SkillPrecisionNeedle(): base("s_precisionneedle")
    {
        name = "Precision Needle";
        type = SkillType.OFFENSIVE;
        element = SkillElement.PIERCE;
        targetting = TargettingType.ENEMY_TARGET;
        cost = 2;
        cooldown = 2;
    }
    protected override void Execute(BattleEngine battleEngine, BattleActor user, BattleActor target, float targetEfficiency)
    {
        int damage = 1;
        
        if (targetEfficiency >= 1.2f) damage = 5;
        else if (targetEfficiency >= 0.90f) damage = 3;
        else if (targetEfficiency >= 0.6f) damage = 2;

        battleEngine.DoDamage(damage, user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, BattleActor user, BattleActor target)
    {
        return -1;
    }

    public override string Description()
    {
        return "- Does " + Utility.ATK("1-5DMG") + " (ignoring all effects and DEF) depending on Target Efficiency";
    }
}