using System.Collections.Generic;

public partial class BattleSlime: BattleMonster
{
    public readonly string[] analyzeInfo = {"Just a wee lil' slimo. Its weak spot is its face.\n",
    "Targets party members from left to right, moving to the next each upcoming turn.", " *BLORB*"};

    private int targetCycle = 0;

    public BattleSlime(): base (
        MonsterId.Slime, "Slime",
        1, 10, 10, 2, 1, 2,
        new List<BattleSkill>
        {
            SkillDatabase.GetSkillData(SkillId.BasicAttack)
        }
    ) {}

    public override void LoadUpcomingTurn(BattleEngine battleEngine)
    {
        targetCharacter = targetCycle;
        targetCycle = (targetCycle + 1) % battleEngine.GetPartySize();
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