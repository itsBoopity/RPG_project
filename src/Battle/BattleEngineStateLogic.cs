using Godot;

/// Holds the ControlState logic of BattleEngine.
public partial class BattleEngine : Control
{
    private ControlState state;

    /// <summary>
    /// Sets the ControlState and updates UI to reflect that.
    /// </summary>
    private void SetState(ControlState state)
    {
        GD.Print($"Entering {state}");
        this.state = state;
        Ui.UpdateInfoLabel(state);
    }
    private void EnterState(ControlState newState)
    {
        if (state == ControlState.PLAYER_DEFAULT)
        {
            EnterPlayerDefaultState();
        }
        if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            EnterTargetState();
        }
        else if (state == ControlState.PLAYER_SELECTING_ENEMY_CUSTOMWINDOW)
        {
            EnterEnemySelectCustomSkillWindowState();
        }
        else if (state == ControlState.PLAYER_CUSTOMWINDOW)
        {
            EnterCustomSkillWindowState();
        }
        else if (state == ControlState.ENEMY_TURN)
        {
            EnterEnemyState();
        }
        else if (state == ControlState.END_SCREEN)
        {
            EnterEndScreenState();
        }
    }

    /// <summary>
    /// Cleans up data and ui after leaving state.
    /// </summary>
    /// <param name="exitedState"> The state to exit from. </param>
    /// <param name="cancelling">   Whether current mode is being exited by cancelling the previous mode,
    ///                             or if transitioning normally through intended input flow. </param>  
    private void ExitState(ControlState exitedState, bool cancelling = false)
    {
        if (exitedState == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            ExitTargetState();
        }
        else if (exitedState == ControlState.PLAYER_SELECTING_ENEMY_CUSTOMWINDOW)
        {
            ExitEnemySelectCustomWindowState();
        }
        else if (exitedState == ControlState.PLAYER_CUSTOMWINDOW)
        {
            ExitCustomSkillWindowState(cancelling);
        }
    }


    
    /// <summary>
    /// Exits current state and cleans up after it.
    /// </summary>
    /// <param name="cancelling">   Whether current mode is being exited by cancelling the previous mode,
    ///                             or if transitioning normally through intended input flow. </param>  
    private void ExitCurrentState(bool cancelling = false)
    {
        ExitState(state, cancelling);
    }

    /// <summary>
    /// If player is in control, cancels current input and sets PLAYER_DEFAULT.
    /// Otherwise does nothing.
    /// </summary>
    private void PlayerExitStateIfPossible()
    {
        if (state != ControlState.ENEMY_TURN)
        {
            SwitchState(ControlState.PLAYER_DEFAULT, true);
        }
    }
    
    /// <summary>
    /// Switches to new state, then cleans up after old state afterwards.
    /// </summary>
    /// <param name="newState"> The state to switch to. </param>
    /// <param name="cancelling">   Whether current mode is being exited by cancelling the previous mode,
    ///                             or if transitioning normally through intended input flow. </param>  
    private void SwitchState(ControlState newState, bool cancelling = false)
    {
        ControlState previous = state;
        state = newState;
        ExitState(previous, cancelling);
        EnterState(newState);
    }  

    private void EnterPlayerDefaultState()
    {
        SetState(ControlState.PLAYER_DEFAULT);
        Ui.ShowMostUI();
    }
    
    private void EnterTargetState()
    {
        SetState(ControlState.PLAYER_TARGETTING_ENEMY);
        Ui.ShowReticle();

        foreach (BattleMonster monster in monsters.GetAll())
        {
            monster.SetTargettingAttack();
            monster.ShowEstimate(selectedSkill.EstimateDamage(GetCurrentPartyMember(), monster));
        }
        Ui.HideMostUI();
    }

    private void EnterEnemySelectCustomSkillWindowState()
    {
        SetState(ControlState.PLAYER_SELECTING_ENEMY_CUSTOMWINDOW);
        foreach (BattleMonster monster in monsters.GetAll())
        {
            monster.SetTargettingSelect();
            monster.ShowEstimate(selectedSkill.EstimateDamage(GetCurrentPartyMember(), monster));
        }
        Ui.HideMostUI();
    }

    /// <summary>
    /// Enter custom skill window mode.
    /// </summary>
    /// <param name="data">BattleInteractionData to inititate skill window with. If null, creates
    ///                    a minimal BattleInteractionData with only user.</param>
    private void EnterCustomSkillWindowState(BattleInteractionData data = null)
    {
        SetState(ControlState.PLAYER_CUSTOMWINDOW);
        data ??= new BattleInteractionData(GetCurrentPartyMember());
        SkillCustomWindow window = selectedSkill.GetWindow();
        window.CancelWindow += PlayerExitStateIfPossible;
        window.ReturnData += PlayerExecuteWindowAction;
        Ui.OpenCustomWindow(window, bF, data);
    }

    private void EnterEndScreenState()
    {
        SetState(ControlState.END_SCREEN);
        Ui.ShowEndScreen();
    }

    private void EnterEnemyState()
    {
        SetState(ControlState.ENEMY_TURN);
        Ui.HideMostUI();
    }

    public void ExitTargetState()
    {
        Ui.DisableReticle();
        foreach (BattleMonster monster in monsters.GetAll())
        {
            monster.DisableTargetting();
            monster.HideEstimate();
        }
        Ui.HideActionDetail();
        Ui.ShowMostUI();
    }

    public void ExitEnemySelectCustomWindowState()
    {
        foreach (BattleMonster monster in monsters.GetAll())
        {
            monster.DisableTargetting();
            monster.HideEstimate();
        }
        Ui.HideActionDetail();
        Ui.ShowMostUI();
    }

    public void ExitCustomSkillWindowState(bool cancelling)
    {
        if (cancelling)
        {
            Ui.CancelCustomWindow();
        }
        else
        {
            Ui.CloseCustomWindow();
        }
        Ui.HideActionDetail();
    }  
}
