using Godot;

/// <summary>
/// A class representing a player character during battles.
/// </summary>
public partial class BattleCharacter: BattleActor
{
    [Signal]
    new public delegate void TookDamageEventHandler(BattleCharacter who, BattleActor damageDealer, int damage);

    public CharacterEnum Who { get; private set; }

    public BattleCharacter(CharacterStats characterStats)
    {
        Who = characterStats.who;
        Stats = characterStats;
    }
}