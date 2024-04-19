public partial class SlimeStats: MonsterStats
{
    private int targetCycle = 0;

    public override void LoadUpcomingTurn(BattleFieldData bF)
    {
        EmitSignal(SignalName.SignalIntent, targetCycle, 0);
        targetCycle = (targetCycle + 1) % bF.party.Count;
    }

    public override void AppendageHit(MonsterAppendage appendage)
    {
        if (appendage.appendageId == 1)
        {
            EmitSignal(SignalName.SignalHitResult, 1.0f);
        }
        else
        {
            EmitSignal(SignalName.SignalHitResult, 0.5f);
        }
    }
}