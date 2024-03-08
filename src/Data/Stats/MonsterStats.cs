using Godot;

public abstract partial class MonsterStats: BattleActorStats
{
    [Signal]
    public delegate void SignalIntentEventHandler(int targetIndex, int skillIndex);
    [Signal]
    public delegate void SignalHitResultEventHandler(float appendageCoef);

    [Export]
    public MonsterId Id { get; set; }

    [Export]
    public static string[] AnalysisInfo { get; set; }

    /// <summary>
    /// The default damage modifier that gets used when non targetting offensive skills are used
    /// </summary>
    [Export]
    public float DefaultAppendageCoef { get; set; }

    public abstract void LoadUpcomingTurn(CharacterRack party, CharacterRack bench, MonsterRack monsters);

    public abstract void AppendageHit(MonsterAppendage appendage);
}