using Godot;

public partial class Slime: Monster
{
    public readonly string[] analyzeInfo = {"Just a wee lil' slimo. Its weak spot is its face.\n",
    "Targets party members from left to right, moving to the next each upcoming turn.", " *BLORB*"};
    public static int GetID() { return 0; }

    private int targetCycle = 0;

    public Slime()
    {
        name = "Slime";
        maxHp = 10;
        stack = 2;
        hp = 10;
        atk = 2;
        def = 1;
        spd = 2;

        skills.Add(new BasicAttack());
    }

    public override void LoadUpcomingTurn(BattleEngine battleEngine)
    {
        targetCharacter = targetCycle;
        targetCycle = (targetCycle + 1) % battleEngine.party.Count;
        targetSkill = 0;
    }

    protected override void TargetHit(MonsterTarget target)
    {
        if (target.Name == "Effective")
        {
            NotifyBattleEngine(1f);
        }
        else
        {
            NotifyBattleEngine(0.5f);
        }
    }
}