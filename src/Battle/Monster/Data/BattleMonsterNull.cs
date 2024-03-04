using Godot;
using System.Collections.Generic;

public partial class BattleMonsterNull: BattleMonster
{
    private BattleMonsterNull(): base (
        MonsterId.Null, "Null",
        0, 0, 0, 0, 0, 0,
        new List<BattleSkill> {}
    ) {}

    public override void LoadUpcomingTurn(BattleEngine battleEngine) {}

    protected override void AppendageHit(MonsterAppendage target) {}

}