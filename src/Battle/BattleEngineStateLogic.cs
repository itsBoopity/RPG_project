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
        this.state = state;
        Ui.UpdateInfoLabel(state);
    }

    private void EnterPlayerDefaultMode()
    {
        SetState(ControlState.PLAYER_DEFAULT);
        Ui.ShowMostUI();
    }
    
    private void EnterTargetMode()
    {
        SetState(ControlState.PLAYER_TARGETTING_ENEMY);
        Ui.ShowReticle();

        foreach (BattleMonster monster in monsters.GetAll())
        {
            monster.targettingEnabled = true;
            monster.ShowEstimate(selectedSkill.EstimateDamage(GetCurrentPartyMember(), monster));
        }
        Ui.HideMostUI();
    }
    
    private void EnterTargetAllyMode()
    {
        SetState(ControlState.PLAYER_TARGETTING_ALLY);
        // TODO: Hide party, model,skill, skillDesc UI and show estimated damage on enemies;
    }

    private void EnterCustomSkillWindowMode()
    {
        SetState(ControlState.PLAYER_SKILL_CUSTOMWINDOW_STATE);
        BattleFieldData bF = new(party, bench, monsters);
        Ui.OpenCustomWindow(selectedSkill.GetWindow(), bF, GetCurrentPartyMember());
    }

    private void EnterEnemyTurn()
    {
        SetState(ControlState.ENEMY_TURN);  
        Ui.HideMostUI();
    }

    // <summary> 
    // Exits the player out of whatever mode they're in and sets them back to PLAYER_TURN (if currently in a state of control)
    //</summary>
    private void ExitCurrentMode()
    {
        selectedSkill = null;
        if (state == ControlState.PLAYER_TARGETTING_ALLY)
        {
            SetState(ControlState.PLAYER_DEFAULT);
        }
        else if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            SetState(ControlState.PLAYER_DEFAULT);
            Ui.DisableReticle();
            foreach (BattleMonster monster in monsters.GetAll())
            {
                monster.targettingEnabled = false;
                monster.HideEstimate();
            }
            Ui.HideActionDetail();
            Ui.ShowMostUI();
        }
        else if (state == ControlState.PLAYER_SKILL_CUSTOMWINDOW_STATE)
        {
            Ui.CloseCustomWindow();
            SetState(ControlState.PLAYER_DEFAULT);
        }
    }
}
