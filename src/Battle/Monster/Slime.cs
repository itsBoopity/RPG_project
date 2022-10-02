using Godot;

public class Slime: Monster
{
    public readonly string[] analyzeInfo = {"Just a wee lil' slimo. Its weak spot is its face.\n",
    "Always targets the 1st party member.", " *BLORB*"};
    public static int GetID() { return 0; }
    public Slime()
    {
        
        name = "Slime";
        maxHP = 10;
        stack = 2;
        HP = 10;
        ATK = 2;
        DEF = 1;
        SPD = 2;

        skills.Add(new BasicAttack());
    }

    public override void LoadUpcomingTurn(BattleEngine battleEngine)
    {
        targetCharacter = 0;
        targetSkill = 0;
    }

    protected override void TargetHit(MonsterTarget target)
    {
        if (target.Name == "Weak")
            NotifyBattleEngine(1f);
        else
            NotifyBattleEngine(0.5f);
    }
    
}