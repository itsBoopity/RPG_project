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
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return 0;
    }

    public override string Description()
    {
        return "T_SKL_BASICDEFEND_DESC";
    }
}