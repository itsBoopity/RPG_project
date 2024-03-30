using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class BattleUI : Control
{
    [Signal]
    public delegate void ActionDetailHiddenEventHandler();

	private Reticle reticle;
    private Node partyNode;
    private CanvasItem skillNode;
    private CanvasItem basicSkillNode;
    private CharacterModelRack characterModels;
    private InfoLabel infoLabel;

    /// <summary>
    /// Used to refresh the skill description currently being displayed when switching characters
    /// as the SkillBoxes themselves don't have any method to re-check where the mouse is.
    /// </summary>
    private int lastViewedActionIndex = -1;
    /// <summary>
    /// Similar to lastViewedAction, but for when the action used is a basic skill.
    /// Is null if lastViewed action was a non-basic skill.
    /// </summary>
    private BattleSkill lastViewedActionBasic = null;
    private SkillDescription skillDesc;

    public FadingMessage characterMessage;
    public FadingMessage centerMessage;

    private TurnAnnouncement turnAnnouncement;
    private DamageCounter characterDamageCounter;
    private AnimationPlayer animationPlayer;

    private Control skillCustomWindow;

	public override void _EnterTree()
    {
        animationPlayer ??= GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Stop();
        animationPlayer.Play("OnEnter");
    }

	public override void _Ready()
	{
		reticle = GetNode<Reticle>("%Reticle");
        partyNode = GetNode("%PartyBar");
        characterModels = GetNode<CharacterModelRack>("%CharacterModels");
        skillNode = GetNode<CanvasItem>("%Skills");
        basicSkillNode = GetNode<CanvasItem>("%BasicSkills");
        infoLabel = GetNode<InfoLabel>("%InfoLabel");
        skillDesc = GetNode<SkillDescription>("%SkillDescription");
        characterMessage = GetNode<FadingMessage>("%CharacterMessage");
        centerMessage = GetNode<FadingMessage>("%CenterMessage");
        turnAnnouncement = GetNode<TurnAnnouncement>("%TurnAnnouncement");
        characterDamageCounter = GetNode<DamageCounter>("%CharacterDamageCounter");
        skillCustomWindow = GetNode<Control>("%SkillCustomWindow");
	}

    public void DisplayDamageCharacter(int damage)
    {
        characterDamageCounter.Play(damage);
    }

	public void UpdateInfoLabel(ControlState state)
	{
		infoLabel.UpdateUI(state);
	}

	public void ShowReticle()
	{
		reticle.ShowReticle();
	}
	public void DisableReticle()
	{
		reticle.Disable();
	}

	public void PlayTurnAnnouncement()
	{
		turnAnnouncement.Play();
	}
	public void StopTurnAnnouncement()
	{
		turnAnnouncement.Stop();
	}

	//Updates all UI using each of the standalone UpdateUI
    public void UpdateAll(CharacterRack party, BattleCharacter selectedCharacter)
    {
        UpdateCharacterBars(party);
        SwapCharacterModel(selectedCharacter);
        UpdateSkills(selectedCharacter);
    }
    public void UpdateCharacterBars(CharacterRack party)
    {
        for (int i=0; i<3; i++)
        {
            if (i < party.Count)
            {
                partyNode.GetChild<CharacterBar>(i).Update(party[i]);
            }
            else
            {
                partyNode.GetChild<CharacterBar>(i).Hide();
            }
        }
    }

    public void UpdateCharacterBar(BattleCharacter character)
    {
        foreach (CharacterBar bar in partyNode.GetChildren().Cast<CharacterBar>())       
        {
            if (bar.Who == character.Who)
            {
                bar.Update(character);
                break;
            }
        }
    }

    public void SwapCharacterModel(BattleCharacter selectedCharacter)
    {
        characterModels.ShowCharacter(selectedCharacter.Who);
    }
    public void HideCharacterModel()
    { 
        characterModels.FadeCharacter();
    }

    public void UpdateSkills(BattleCharacter selectedCharacter)
    {
        for (int i=0; i<5; i++)
        {
            if (i < selectedCharacter.Skills.Count)
            {
                skillNode.GetChild<SkillBoxUI>(i).Update(selectedCharacter, selectedCharacter.Skills[i]);
            }
            else
            {
                skillNode.GetChild<SkillBoxUI>(i).Empty();
            }
        }
    }
    public void ViewActionDetail(BattleSkill skill, int index, BattleFieldData bF, BattleCharacter owner)
    {
        if (skill.Type == SkillType.BASIC)
        {
            lastViewedActionBasic = skill;
            lastViewedActionIndex = -1;
        }
        else
        {
            lastViewedActionIndex = index;
            lastViewedActionBasic = null;
        }
        skillDesc.ShowSkill(skill, bF, owner);
    }

    public void HideActionDetail()
    {
        lastViewedActionIndex = -1;
        skillDesc.HideSkill();
    }

    public void HideMostUI()
    {
        characterModels.Hide();
        skillNode.Hide();
        basicSkillNode.Hide();
    }
    public void ShowMostUI()
    {
        characterModels.Show();
        skillNode.Show();
        basicSkillNode.Show();
    }

    public void PrintCenterMessage(string text)
    {
        centerMessage.ShowText(text);
    }
    public void PrintCharacterMessage(string text)
    {
        characterMessage.ShowText(text);
    }

    public void SelectCharacter(BattleCharacter character, int index, BattleFieldData bF)
    {
        foreach (CharacterBar bar in partyNode.GetChildren().Cast<CharacterBar>())
        {
            bar.Deselect();
        }
        partyNode.GetChild<CharacterBar>(index).Select();

        // Update the currently viewed skill to the one of the new character.
        if (lastViewedActionBasic != null)
        {
            ViewActionDetail(lastViewedActionBasic, lastViewedActionIndex, bF, character);
        }
        else
        {
            if (lastViewedActionIndex >= character.Skills.Count)
            {
                HideActionDetail();
            }
            else if (lastViewedActionIndex >= 0)
            {
                ViewActionDetail(character.Skills[lastViewedActionIndex], lastViewedActionIndex, bF, character);
            }
        }
    }

    public void CharacterTookDamage(BattleCharacter character, BattleActor damageDealer, int damage)
    {
        characterDamageCounter.Play(damage);
        foreach (CharacterBar bar in partyNode.GetChildren().Cast<CharacterBar>())
		{
			if (bar.Who == character.Who)
			{
				bar.Update(character);
				bar.ShakeHP();
				break;
			}
		}
    }

    public void UpdateCharacterHP(BattleCharacter character)
    {
        foreach (CharacterBar bar in partyNode.GetChildren().Cast<CharacterBar>())
		{
			if (bar.Who == character.Who)
			{
				bar.Update(character);
				bar.ShakeHP();
				break;
			}
		}
    }
    public void ShakeStackCount(int index)
    {
        partyNode.GetChild<CharacterBar>(index).ShakeStack();
    }

    public void PlayVfx(AnimatedSpriteOneOff animation, Vector2 position)
    {
        AddChild(animation);
        animation.GlobalPosition = position;
    }

    public void PlayerAttackedVfx(AnimatedSpriteOneOff animation, BattleCharacter character)
    {
        characterModels.ShowCharacter(character.Who);
        PlayVfx(animation, characterModels.Position - new Vector2(0, 400));
        characterModels.Show();
    }

    public void OpenCustomWindow(SkillCustomWindow window, BattleFieldData bF, BattleCharacter user)
    {
        ClearCustomWindow();
        skillCustomWindow.Show();
        skillCustomWindow.AddChild(window);
        window.Open(bF, user);
    }

    public void CloseCustomWindow()
    {
        skillCustomWindow.Hide();
        ClearCustomWindow();
    }

    private void ClearCustomWindow()
    {
        foreach (SkillCustomWindow child in skillCustomWindow.GetChildren().Cast<SkillCustomWindow>())
        {
            child.Close();
            child.QueueFree();
        }
    }
}
