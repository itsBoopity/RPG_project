/// <summary>
/// Enum describing which control UI is used by each skill.
/// </summary>
public enum TargettingType
{
    /// <summary>
    /// Activate the skill without requiring any specialized input, only a confirmation. 
    /// </summary>
    NONE,
    SELF,
    ALLY_TARGET,
    ALLY_SELECT,
    /// <summary>
    /// Show reticle to target enemy with and wait for appendage hit.
    /// </summary>
    ENEMY_TARGET,
    ENEMY_SELECT,
    /// <summary>
    /// Lets player select a monster, followed by displaying a custom window to facilitate further input. Finally executes skill.
    /// </summary>
    ENEMY_SELECT_CUSTOMWINDOW,

    /// <summary>
    /// Displays a custom window to facilitate further input.
    /// </summary>
    CUSTOMWINDOW
}