public partial class SkillTemplate: BattleSkillData
{
    public SkillTemplate(): base(
        SkillId.Null, "SKILL_NULL", 0, 0, 0, false, 0, 0, false
    ) {}
    
    public override void Execute(BattleFieldData bf, BattleInteractionData bInteraction)
    {
    
    }
    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return CalculationFormula.BasicDamage(user.GetStrength(), target.GetDefense(), target.GetAffinity(Element));
    }
    public override string Description()
    {
        return "";
    }

    // Overwrite if the skill needs to reset internal parameters
    // public override void Reset()
    // {
    //     base.Reset();
    //     // Rest of the code
    // }

}