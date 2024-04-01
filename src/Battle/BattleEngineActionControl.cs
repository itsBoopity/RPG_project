using Godot;

// Contains methods related to viewing and selecting actions.
public partial class BattleEngine : Control
{
    public void ViewSkill(int index)
    {
        if (index < GetCurrentPartyMember().Skills.Count)
        {
            ViewAction(GetCurrentPartyMember().Skills[index], index);
        }
    }
    public void ViewBasicSkill(int index)
    {
        if (index < basicSkills.Length)
        {
            ViewAction(basicSkills[index], index);
        }
    }
    public void ViewAction(BattleSkill action, int index)
    {
        Ui.ViewActionDetail(action, index, bF, GetCurrentPartyMember());
        foreach (BattleMonster monster in monsters.GetAll())
        {
            monster.ShowEstimate(action.EstimateDamage(GetCurrentPartyMember(), monster));
        }
    }

    public void SelectSkill(int index)
    {
        if (index < GetCurrentPartyMember().Skills.Count && GetCurrentPartyMember().TurnActive)
        {
            SelectAction(GetCurrentPartyMember().Skills[index], index);
        }
    }
    public void SelectBasicSkill(int index)
    {
        if (index < basicSkills.Length && GetCurrentPartyMember().TurnActive)
        {
            SelectAction(basicSkills[index], index);
        }        
    }

    public void SelectAction(BattleSkill action, int index)
    {
        SkillUsableResult isUsable = action.IsUsable(GetCurrentPartyMember());
        if (isUsable != SkillUsableResult.USABLE)
        {
            sfx.ErrorSound();
            if (isUsable == SkillUsableResult.IN_COOLDOWN)
            {
                Ui.PrintCharacterMessage("T_B_MSG_CD");
            } 
            else if (isUsable == SkillUsableResult.NOT_ENOUGH_STACKS)
            {
                Ui.ShakeStackCount(selectedCharacter);
                Ui.PrintCharacterMessage("T_B_MSG_NOSTACK");
            }
        }
        else
        {
            sfx.RollClickPitchIndex(index);
            selectedSkill = action;
            Ui.ViewActionDetail(action, index, bF, GetCurrentPartyMember());
            if (action.Targetting == TargettingType.ENEMY_TARGET)
            {
                SwitchState(ControlState.PLAYER_TARGETTING_ENEMY);
            }
            else if (action.Targetting == TargettingType.ENEMY_SELECT_CUSTOMWINDOW)
            {
                SwitchState(ControlState.PLAYER_SELECTING_ENEMY_CUSTOMWINDOW);
            }
            else if (action.Targetting == TargettingType.CUSTOMWINDOW)
            {
                SwitchState(ControlState.PLAYER_CUSTOMWINDOW);
            }
        }
    }
}
