using Godot;

public class Slime: Monster
{
    public readonly string[] analyzeInfo = {"Just a wee lil' slimo. Their weak spot is their face.",
    "Always targets the 1st party member."};
    public static int GetID() { return 0; }
    public Slime()
    {
        name = "Slime";
        maxHP = 10;
        HP = 10;
        ATK = 2;
        DEF = 1;

        skills.Add(new BasicAttack());
    }

    public override void LoadUpcomingTurn(BattleEngine battleEngine)
    {
        targetCharacter = 0;
        targetSkill = 0;
    }
}