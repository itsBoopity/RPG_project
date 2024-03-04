using Godot;
using System.Collections.Generic;

public partial class BattleSlime: BattleMonster
{
    public readonly string[] analyzeInfo = {"Just a wee lil' slimo. Its weak spot is its face.\n",
    "Targets party members from left to right, moving to the next each upcoming turn.", " *BLORB*"};

    private int targetCycle = 0;

    private BattleSlime(): base (
        MonsterId.Slime, "Slime",
        1, 10, 10, 2, 1, 2,
        new List<BattleSkill>
        {
            SkillDatabase.GetSkillData(SkillId.BasicAttack)
        }
    ) {}

    /// <summary>
    /// Instances and returns the .tscn Node containing this monster.
    /// </summary>
    public static BattleMonster Instance()
    {
        return GD.Load<PackedScene>("res://Objects/Monster/Basic/Slime.tscn").Instantiate<BattleMonster>();
    }

    public override void LoadUpcomingTurn(BattleEngine battleEngine)
    {
        targetCharacter = targetCycle;
        targetCycle = (targetCycle + 1) % battleEngine.GetPartySize();
        targetSkill = 0;
    }

    protected override void AppendageHit(MonsterAppendage appendage)
    {
        if (appendage.Name == "Effective")
        {
            EmitSignal(SignalName.Attacked, this, 1.0f);
        }
        else
        {
            EmitSignal(SignalName.Attacked, this, 0.5f);
        }
    }
}