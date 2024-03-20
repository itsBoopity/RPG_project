using Godot;

[GlobalClass]
public partial class SkillBasicDefend: BattleSkillData
{
    public SkillBasicDefend(): base(
        SkillId.Defend,
        "T_SKL_BASICDEFEND_TITLE",
        SkillType.BASIC,
        SkillElement.NONE,
        TargettingType.NONE,
        false,
        0,
        0,
        false
    ) {}

    public override void Execute(BattleFieldData bf, BattleInteractionData bInteraction)
    {
        bInteraction.user.ChangeStack(2);
        bInteraction.target.SustainDamage(bInteraction.user, Utility.BasicDamageFormula(bInteraction.user.GetAttack(), bInteraction.target.GetDefense(), bInteraction.appendageCoef));
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense());
    }

    public override string Description()
    {
        return "T_SKL_BASICATTACK_DESC";
    }
}