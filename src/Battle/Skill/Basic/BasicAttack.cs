using Godot;

[System.Serializable]
public class BasicAttack: BattleSkill
{
    public BasicAttack()
    {
        name = "Basic Attack";
        type = SkillType.BASIC;
        cost = 0;
        cooldown = 0;
        textureX = 0;
        textureY = 0;
    }
    public override void Execute(BattleFigure user, BattleFigure target)
    {
        int damage = (target.ATK - target.DEF < 1) ? 1 : target.ATK - target.DEF;
        target.HP -= damage;

        //Play animation
    }

    public override string Description()
    {
        return "- Deal ATK damage\n- Generates [st]+1 STA[/st]";
    }
}