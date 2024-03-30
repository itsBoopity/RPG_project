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
        if (bF.party.ContainsBattleCharacter((BattleCharacter)bI.target))
        {
            bF.party.Swap((BattleCharacter)bI.user, (BattleCharacter)bI.target);
        }
        else
        {
            int benchPosition = bI.target.GetIndex();
            bF.bench.Remove((BattleCharacter)bI.target);
            bF.party.Replace((BattleCharacter)bI.user, (BattleCharacter)bI.target);
            bF.bench.Add((BattleCharacter)bI.user);
            bF.bench.MoveChild((BattleCharacter)bI.user, benchPosition);
            bI.target.TurnActive = false;
        }

        bI.target.ChangeStack(2);
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