using Godot;

[GlobalClass]
public partial class SkillBasicAttack: BattleSkillData
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

    public override void Execute(BattleFieldData bf, BattleInteractionData bInteraction)
    {
        bInteraction.user.ChangeStack(1);
        int damageStat = bInteraction.user.GetStrength();
        if (bInteraction.user.GetIntelligence() > damageStat)
        {
            damageStat = bInteraction.user.GetIntelligence();
        }

        bInteraction.target.SustainDamage(bInteraction.user, CalculationFormula.BasicDamage(damageStat, bInteraction.target.GetDefense(), bInteraction.target.GetAffinity(bInteraction.user.Element), bInteraction.appendageCoef));
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        int damageStat = user.GetStrength();
        if (user.GetIntelligence() > damageStat)
        {
            damageStat = user.GetIntelligence();
        }
        return CalculationFormula.BasicDamage(damageStat, target.GetDefense(), target.GetAffinity(user.Element));
    }

    public override string Description()
    {
        return "T_SKL_BASICATTACK_DESC";
    }
}