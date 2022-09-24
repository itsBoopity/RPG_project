using Godot;

[System.Serializable]
public class BasicAttack: BattleSkill
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
    protected override void Execute(BattleEngine battleEngine, BattleFigure user, BattleFigure target, float targetEfficiency)
    {
        user.stack += 1;
        battleEngine.DoDamage(Utility.BasicDamageFormula(user.GetATK(), target.GetDEF(), targetEfficiency), user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, BattleFigure user, BattleFigure target)
    {
        return Utility.BasicDamageFormula(user.GetATK(), target.GetDEF());
    }

    public override string Description()
    {
        return "- Do " + Utility.ATK("ATK") + " damage\n- Generates " + Utility.STA("+1STA");
    }
}