using Godot;

[GlobalClass]
public partial class SkillBasicAttack: BattleSkillData
{
    public SkillBasicAttack(): base(
        SkillId.BasicAttack,
        "T_SKL_BASICATTACK_TITLE",
        SkillType.BASIC,
        SkillElement.VARIABLE,
        TargettingType.ENEMY_TARGET,
        false,
        0,
        0,
        false
    ) {}

    public override SkillElement GetDisplayElement(BattleFieldData bF, BattleCharacter user)
    {
        return user.Element;
    }

    public override void Execute(BattleFieldData bF, BattleInteractionData bI)
    {
        bI.user.ChangeStack(1);
        int damageStat;
        if (bI.user.Element.IsMagical())
        {
            damageStat = bI.user.GetIntelligence();
        }
        else
        {
            // Note that: if element is neither, defaults to using Strength.
            damageStat = bI.user.GetStrength();
        }

        bI.target.SustainDamage(bI.user, CalculationFormula.BasicDamage(damageStat, bI.target.GetDefense(), bI.target.GetAffinity(bI.user.Element), bI.appendageCoef));
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        int damageStat;
        if (user.Element.IsMagical())
        {
            damageStat = user.GetIntelligence();
        }
        else
        {
            damageStat = user.GetStrength();
        }
        return CalculationFormula.BasicDamage(damageStat, target.GetDefense(), target.GetAffinity(user.Element));
    }

    public override string Description(BattleFieldData bF, BattleCharacter user)
    {
        if (user.Element.IsMagical())
        {
            return "T_SKL_BASICATTACK_DESC_MAGICAL";
        }
        else
        {
            return "T_SKL_BASICATTACK_DESC_PHYSICAL";
        }
    }
}