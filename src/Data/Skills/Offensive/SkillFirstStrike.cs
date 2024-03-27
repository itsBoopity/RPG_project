using Godot;

[GlobalClass]
public partial class SkillFirstStrike: BattleSkillData
{
    private bool first = true;
    public SkillFirstStrike(): base(
        SkillId.FirstStrike,
        "T_SKL_FIRSTSTRIKE_TITLE",
        SkillType.OFFENSIVE,
        SkillElement.BLUNT,
        TargettingType.ENEMY_TARGET,
        false,
        3,
        2,
        false
    ) {}

    public override void Initialize()
    {
        first = true;
    }

    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
        GD.Print(first);
        int damage;
        if (first) {
            damage = CalculationFormula.BasicDamage(bI.user.GetStrength(), bI.target.GetDefense(), bI.target.GetAffinity(Element), bI.appendageCoef, 2.0f);
            first = false;
        }
        else
            damage = CalculationFormula.BasicDamage(bI.user.GetStrength(), bI.target.GetDefense(), bI.target.GetAffinity(Element), bI.appendageCoef, 1.5f);

        bI.target.SustainDamage(bI.user, damage);
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        if (first)
            return CalculationFormula.BasicDamage(user.GetStrength(), target.GetDefense(), target.GetAffinity(Element), 1.0f, 2.0f);
        else
            return CalculationFormula.BasicDamage(user.GetStrength(), target.GetDefense(), target.GetAffinity(Element), 1.0f, 1.5f);
    }

    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        return "T_SKL_FIRSTSTRIKE_DESC";
    }
}