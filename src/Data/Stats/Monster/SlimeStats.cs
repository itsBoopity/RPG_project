public partial class SlimeStats: MonsterStats
{
    private int targetCycle = 0;

    public override void LoadUpcomingTurn(CharacterRack party, CharacterRack bench, MonsterRack monsters)
    {
        EmitSignal(SignalName.SignalIntent, targetCycle, 0);
        targetCycle = (targetCycle + 1) % party.Count;
    }

    public override void AppendageHit(MonsterAppendage appendage)
    {
        if (appendage.Name == "Effective")
        {
            EmitSignal(SignalName.SignalHitResult, 1.0f);
        }
        else
        {
            EmitSignal(SignalName.SignalHitResult, 0.5f);
        }
    }
}