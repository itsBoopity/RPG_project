using Godot;

[GlobalClass]
public partial class SkillBasicAnalyze: BattleSkillData
{
    public SkillBasicAnalyze(): base(
        SkillId.Swap,
        "T_SKL_ANALYZE_TITLE",
        SkillType.BASIC,
        SkillElement.NONE,
        TargettingType.ENEMY_SELECT_CUSTOMWINDOW,
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
        return "T_SKL_ANALYZE_DESC";
    }

    public override SkillCustomWindow GetCustomWindow()
    {
        return GD.Load<PackedScene>("res://Objects/UI/Battle/SkillCustomWindow/SkillCustomWindowAnalyze.tscn").Instantiate<SkillCustomWindow>();
    }

    public override AnimatedSpriteOneOff GetAnimation()
    {
        return GD.Load<PackedScene>("res://Objects/VFX/Analyze.tscn").Instantiate<AnimatedSpriteOneOff>();
    }
}