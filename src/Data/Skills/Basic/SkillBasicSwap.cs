using Godot;

[GlobalClass]
public partial class SkillBasicSwap: BattleSkillData
{
    public SkillBasicSwap(): base(
        SkillId.Swap,
        "T_SKL_SWAP_TITLE",
        SkillType.BASIC,
        SkillElement.NONE,
        TargettingType.CUSTOMWINDOW,
        false,
        0,
        0,
        false
    ) {}

    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
        bF.bench.Remove((BattleCharacter)bI.target);
        bF.party.SwapOut((BattleCharacter)bI.user, (BattleCharacter)bI.target);
        bF.bench.Add((BattleCharacter)bI.user);
        bI.target.TurnActive = false;
        bI.target.ChangeStack(1);
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return 0;
    }

    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_SWAP_DESC";
    }

    public override SkillCustomWindow GetCustomWindow()
    {
        return GD.Load<PackedScene>("res://Objects/UI/Battle/SkillCustomWindow/SkillCustomWindowSwap.tscn").Instantiate<SkillCustomWindow>();
    }
}