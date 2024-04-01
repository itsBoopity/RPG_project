using Godot;

[GlobalClass]
public partial class SkillGemBurst : BattleSkillData
{
    public SkillGemBurst(): base(
        SkillId.GemBurst,
        "T_SKL_GEMBURST_TITLE",
        SkillType.OFFENSIVE,
        SkillElement.BLUNT,
        TargettingType.NONE,
        true,
        0,
        0,
        false
    ) {}
    
    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
		foreach(BattleActor target in bI.targets)
		{
			target.SustainDamage(bI.user, CalculationFormula.BasicDamage(bI.user.GetStrength(), target.GetDefense(), target.GetAffinity(Element)));
		}
    }
    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return CalculationFormula.BasicDamage(user.GetStrength(), target.GetDefense(), target.GetAffinity(Element));
    }
    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_GEMBURST_DESC";
    }

}