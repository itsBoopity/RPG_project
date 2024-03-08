using Godot;

/// <summary>
/// A class representing a player character during battles.
/// </summary>
public partial class BattleCharacter: BattleActor
{
    [Signal]
    new public delegate void TookDamageEventHandler(BattleCharacter who, int damage);

    public CharacterEnum Who { get; private set; }
    public bool TurnActive { get; set; } = true;
    public BattleCharacter(CharacterStats characterStats)
    {
        Who = characterStats.who;
        Stats = characterStats;
    }

    /// <summary>
    /// Counts down parameters at the start of the turn.
    /// </summary>
    public void NextTurn()
    {
        TurnActive = true;
        foreach (BattleSkill skill in Skills)
            if (skill.CurrentCooldown > 0) skill.CurrentCooldown -= 1;
    }
}