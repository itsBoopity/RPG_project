using Godot;
[GlobalClass]
public partial class SkillPrecisionNeedle: BattleSkillData
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
    
    public override void Execute(BattleActor user, BattleActor target, float appendageCoef)
    {
        int damage = 1;
        
        if (appendageCoef >= 1.2f) damage = 5;
        else if (appendageCoef >= 0.90f) damage = 3;
        else if (appendageCoef >= 0.6f) damage = 2;

        target.SustainDamage(user, damage);
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return -1;
    }

    public override string Description()
    {
        return "T_SKL_PRECISIONNEEDLE_DESC";
    }
}