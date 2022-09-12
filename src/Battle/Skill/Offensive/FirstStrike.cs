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
        cost = 3;
        cooldown = 1;
        textureX = 0;
        textureY = 6;

        first = true;
    }
    public override void Execute(BattleFigure user, BattleFigure target)
    {
        int damage;
        if (first)
        {
            damage = target.ATK * 2 - target.DEF;
            first = false;
        }
        else
            damage = (int)(target.ATK * 1.5f) - target.DEF;
        target.HP -= (damage < 0)? 0 : damage;
    }

    public override string Description()
    {
        return "- Do " + Utility.ATK("2*ATK") + " damage on first use\n- Do " + Utility.ATK("1.5*ATK") + " the rest of the combat";
    }

    public new void Reset()
    {
        base.Reset();
        first = true;
    }
}