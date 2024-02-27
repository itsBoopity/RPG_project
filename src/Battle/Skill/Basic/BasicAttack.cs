using Godot;

[System.Serializable]
public partial class BasicAttack: BattleSkill
{
    public BasicAttack()
    {
        name = "Basic Attack";
        type = SkillType.BASIC;
        element = SkillElement.VARIABLE;
        targetting = TargettingType.ENEMY_TARGET;
        cost = 0;
        cooldown = 0;
    }
    protected override void Execute(BattleEngine battleEngine, BattleActor user, BattleActor target, float targetEfficiency)
    {
        user.stack += 1;
        battleEngine.DoDamage(Utility.BasicDamageFormula(user.GetAtk(), target.GetDef(), targetEfficiency), user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, BattleActor user, BattleActor target)
    {
        return Utility.BasicDamageFormula(user.GetAtk(), target.GetDef());
    }

    public override string Description()
    {
        return "- Do " + Utility.ATK("ATK") + " damage\n- Generates " + Utility.STA("+1STA");
    }
}