using Godot;

/// <summary>
/// A class representing a player character during battles.
/// </summary>
public partial class BattleCharacter: BattleActor
{
    [Signal]
    new public delegate void TookDamageEventHandler(BattleCharacter who, BattleActor damageDealer, int damage);

    [Signal]
    new public delegate void StatsChangedEventHandler(BattleCharacter who);

    public CharacterEnum Who { get; private set; }

    public BattleCharacter(CharacterStats characterStats)
    {
        Who = characterStats.Who;
        Stats = characterStats;
    }
}