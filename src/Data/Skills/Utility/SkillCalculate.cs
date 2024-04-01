using Godot;

[GlobalClass]
public partial class SkillCalculate : BattleSkillData
{
    public SkillCalculate(): base(
        SkillId.Calculate,
        "T_SKL_CALCULATE_TITLE",
        SkillType.UTILITY,
		SkillElement.NONE,
		TargettingType.CUSTOMWINDOW,
		false,
		0,
		3,
		true
    ) {}
    
    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
		bI.user.ChangeStack(bI.IntValue);
    }
    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return 0;
    }

    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_CALCULATE_DESC";
    }

     public override SkillCustomWindow GetCustomWindow()
    {
        return GD.Load<PackedScene>("res://Objects/UI/Battle/SkillCustomWindow/SkillCustomWindowCalculate.tscn").Instantiate<SkillCustomWindow>();
    }
}