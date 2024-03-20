using Godot;


/// <summary>
/// An abstract class representing the stats and behavior of a monster.
/// </summary>
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

    public MonsterVisuals Visuals { get; set; }

    public virtual void Initialize() {}

    public abstract void LoadUpcomingTurn(BattleFieldData bf);

    public abstract void AppendageHit(MonsterAppendage appendage);
}