using Godot;

[GlobalClass]
public partial class SkillSimpleMagic: BattleSkillData
{
    public SkillSimpleMagic(): base(
        SkillId.SimpleMagic,
        "T_SKL_SIMPLEMAGIC_TITLE",
        SkillType.OFFENSIVE,
        SkillElement.VARIABLE,
        TargettingType.ENEMY_TARGET,
        false,
        2,
        3,
        false
    ) {}

    public override SkillElement GetDisplayElement(BattleFieldData bF, BattleCharacter user)
    {
        if (Global.Settings.showElementalModuloResult)
        {
            return CalculateElement(user.Stack);
        }
        else
        {
            return SkillElement.VARIABLE;
        }
    }

    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
        bI.target.SustainDamage(bI.user, CalculationFormula.BasicDamage(bI.user.GetIntelligence(), bI.target.GetDefense(), bI.target.GetAffinity(CalculateElement(bI.user.Stack)), bI.FloatValue, 1.5f));
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return CalculationFormula.BasicDamage(user.GetIntelligence(), target.GetDefense(), target.GetAffinity(CalculateElement(user.Stack)), 1.0f, 1.5f);
    }

    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_SIMPLEMAGIC_DESC";
    }

    private SkillElement CalculateElement(int stack)
    {
        return (stack % 3) switch
        {
            0 => SkillElement.FIRE,
            1 => SkillElement.ICE,
            2 => SkillElement.LIGHTNING,
            _ => throw new System.NotImplementedException("This literally shouldn't be possible."),
        };
    }
}