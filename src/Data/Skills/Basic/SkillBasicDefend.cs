using Godot;

[GlobalClass]
public partial class SkillBasicDefend: BattleSkillData
{
    public SkillBasicDefend(): base(
        SkillId.Defend,
        "T_SKL_BASICDEFEND_TITLE",
        SkillType.BASIC,
        SkillElement.NONE,
        TargettingType.SELF,
        false,
        0,
        0,
        false
    ) {}

    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
        bI.user.ChangeStack(2);
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return 0;
    }

    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_BASICDEFEND_DESC";
    }
}