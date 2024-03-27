public partial class StackSpriteStats: MonsterStats
{
    public override void LoadUpcomingTurn(BattleFieldData bF)
    {
        EmitSignal(SignalName.SignalIntent, -1, -1);
    }

    public override void AppendageHit(MonsterAppendage appendage)
    {
        if (appendage.Name == "Effective")
        {
            EmitSignal(SignalName.SignalHitResult, 1.0f);
        }
    }
    
    public override void OnDefeated(BattleActor damageDealer)
    {
        damageDealer.ChangeStack(10);
    }
}