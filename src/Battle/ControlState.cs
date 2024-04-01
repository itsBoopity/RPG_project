public enum ControlState
{
    FULLY_DISABLED,

    /// <summary>
    /// Enemy turn currently taking place.
    /// </summary>
    ENEMY_TURN,
    
    /// <summary>
    /// Player control in default state.
    /// </summary>
    PLAYER_DEFAULT,

    /// <summary>
    /// Player control after selecting skill that uses standard targetting control.
    /// </summary>
    PLAYER_TARGETTING_ENEMY,
    PLAYER_CUSTOMWINDOW,

    /// <summary>
    /// State waiting for player to select enemy, then move to PLAYER_CUSTOMWINDOW for further input. Finally, get input and perform skill.
    /// </summary>
    PLAYER_SELECTING_ENEMY_CUSTOMWINDOW,


    END_SCREEN,
}