using Godot;

// Contains methods related to executing skills of various TargettingType's.
public partial class BattleEngine : Control
{
    /// <summary>
    /// Called at the end of every skill being performed, regardles of TargettingType.
    /// </summary>
    /// <param name="character">The character that performed the action.</param>
    private void CleanUpAfterPerformingAction(BattleCharacter character)
    {
        if (!selectedSkill.Snap)
        {
            UseUpTurn(character);
        }
        Ui.UpdateSkills(character);
        ExitCurrentMode();
    }

    /// <summary>
    /// Offensive skills that use the standard targetting method, but the player missed.
    /// </summary>
    /// <param name="monster">The target that was missed.</param>
    private void PlayerMissEnemyTargetAction(BattleMonster monster)
    {
        BattleCharacter character = GetCurrentPartyMember();
        sfx.FailSound();
        selectedSkill.Miss(character, monster);
        monster.PlayerMissed();
        CleanUpAfterPerformingAction(character);
    }

    /// <summary>
    /// Offensive skills that use the standard targetting method.
    /// </summary>
    /// <param name="monster">The target being attacked.</param>
    /// <param name="appendageCoef">The appendage coefficient resulting from where monster was hit.</param>
    private void PlayerExecuteEnemyTargetAction(BattleMonster monster, float appendageCoef)
    {
        BattleCharacter character = GetCurrentPartyMember();
        BattleInteractionData bI = new BattleInteractionData(character, monster).AddFloatValue(appendageCoef);
        selectedSkill.Use(bF, bI);
        Ui.PlayVfx(selectedSkill.Animation, GetGlobalMousePosition());
        CleanUpAfterPerformingAction(character);
    }

    /// <summary>
    /// Skills that use custom windows that return BattleActor.
    /// </summary>
    /// <param name="actor"></param>
    private void PlayerExecuteWindowAction(BattleInteractionData bI)
    {
        selectedSkill.Use(bF, bI);

        // If BattleCharacter, play on status bar, otherwise play on monster position
        Ui.CloseCustomWindow();
        CleanUpAfterPerformingAction((BattleCharacter)bI.user);
    }
}
