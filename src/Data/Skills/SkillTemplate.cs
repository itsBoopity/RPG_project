public partial class SkillTemplate : BattleSkillData
{
    public SkillTemplate(): base(
        SkillId.Null,
        "T_SKL_TEMPLATE_TITLE",
        SkillType.BASIC,
        SkillElement.NONE,
        TargettingType.NONE,
        false,
        0,
        0,
        false
    ) {}

    // If skill needs to setup at start of battle.
    // public override void Initialize()
    // {
    
    // }
    
    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
    
    }
    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return CalculationFormula.BasicDamage(user.GetStrength(), target.GetDefense(), target.GetAffinity(Element));
    }
    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_TEMPLATE_DESC";
    }

}