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

    public override void Execute(BattleFieldData bf, BattleInteractionData bInteraction)
    {
        int damage;
        if (first) {
            damage = Utility.BasicDamageFormula(bInteraction.user.GetAttack(), bInteraction.target.GetDefense(), bInteraction.appendageCoef, 2);
            first = false;
        }
        else
            damage = Utility.BasicDamageFormula(bInteraction.user.GetAttack(), bInteraction.target.GetDefense(), bInteraction.appendageCoef, 1.5f);

        bInteraction.target.SustainDamage(bInteraction.user, damage);
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        if (first)
            return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), 1, 2.0f);
        else
            return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), 1, 1.5f);
    }

    public override string Description()
    {
        return "T_SKL_FIRSTSTRIKE_DESC";
    }
}