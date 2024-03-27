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
    
    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
        int damage;
        
        if (bI.appendageCoef >= 1.2f) damage = 5;
        else if (bI.appendageCoef >= 1.0f) damage = 3;
        else if (bI.appendageCoef >= 0.6f) damage = 2;
        else damage = 1;

        bI.target.SustainDamage(bI.user, damage);
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return -1;
    }

    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_PRECISIONNEEDLE_DESC";
    }
}