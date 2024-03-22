/// <summary>
/// Enum describing which control UI is used by a skill
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
    CUSTOM
}