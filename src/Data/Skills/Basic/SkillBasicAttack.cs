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

    public override void Execute(BattleActor user, BattleActor target, float appendageCoef)
    {
        user.ChangeStack(1);
        target.SustainDamage(user, Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense(), appendageCoef));
    }

    public override int EstimateDamage(BattleActor user, BattleActor target)
    {
        return Utility.BasicDamageFormula(user.GetAttack(), target.GetDefense());
    }

    public override string Description()
    {
        return "T_SKL_BASICATTACK_DESC";
    }
}