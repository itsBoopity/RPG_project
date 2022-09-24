using Godot;

[System.Serializable]
public class FirstStrike: BattleSkill
{
    private bool first;
    public FirstStrike()
    {
        name = "First Strike";
        type = SkillType.OFFENSIVE;
        element = SkillElement.BLUNT;
        targetting = TargettingType.ENEMY_TARGET;
        cost = 3;
        cooldown = 2;

        first = true;
    }
    protected override void Execute(BattleEngine battleEngine, BattleFigure user, BattleFigure target, float targetEfficiency)
    {
        int damage;
        if (first) {
            damage = Utility.BasicDamageFormula(user.GetATK(), target.GetDEF(), targetEfficiency, 2);
            first = false;
        }
        else
            damage = Utility.BasicDamageFormula(user.GetATK(), target.GetDEF(), targetEfficiency, 1.5f);

        battleEngine.DoDamage(damage, user, target);
    }

    public override int EstimateDamage(BattleEngine battleEngine, BattleFigure user, BattleFigure target)
    {
        if (first)
            return Utility.BasicDamageFormula(user.GetATK(), target.GetDEF(), 1, 2);
        else
            return Utility.BasicDamageFormula(user.GetATK(), target.GetDEF(), 1, 1.5f);
    }

    public override string Description()
    {
        return "- Do " + Utility.ATK("2*ATK") + " damage on first use\n- Do " + Utility.ATK("1.5*ATK") + " the rest of the combat";
    }

    public override void Reset()
    {
        base.Reset();
        first = true;
    }
}