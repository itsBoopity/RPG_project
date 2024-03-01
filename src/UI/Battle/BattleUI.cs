using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Godot;

public partial class BattleUI : Control
{
	private Reticle reticle;
    private Node partyNode;
    private MonsterRack monsterNode;
    private CanvasItem skillNode;
    private CanvasItem basicSkillNode;
    private CharacterModelRack characterModels;
    private InfoLabel infoLabel;

    // Used to refresh the skill description currently being displayed when switching characters
    // as the SkillBoxes themselves don't have any method to re-check where the mouse is.
    private int lastViewedActionIndex = -1;
    private SkillDescription skillDesc;

    public FadingMessage characterMessage;
    public FadingMessage centerMessage;

    private TurnAnnouncement turnAnnouncement;
    private DamageCounter characterDamageCounter;


    private AnimationPlayer animationPlayer;

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
        monsterNode = GetNode<MonsterRack>("%Monsters");
        skillNode = GetNode<CanvasItem>("%Skills");
        basicSkillNode = GetNode<CanvasItem>("%BasicSkills");
        infoLabel = GetNode<InfoLabel>("%InfoLabel");
        skillDesc = GetNode<SkillDescription>("%SkillDescription");
        characterMessage = GetNode<FadingMessage>("%CharacterMessage");
        centerMessage = GetNode<FadingMessage>("%CenterMessage");
        turnAnnouncement = GetNode<TurnAnnouncement>("%TurnAnnouncement");
        characterDamageCounter = GetNode<DamageCounter>("%CharacterDamageCounter");
	}

	public void AddMonster(MonsterModel model)
	{
		monsterNode.AddModel(model);
		model.UpdateHP();
	}

	public void ClearMonsters()
	{
		monsterNode.Clear();
	}

    public void UpdateMonsterModel(Monster monster)
    {
        monster.GetModel().UpdateHP();
    }
    public void DisplayMissMonster(Monster monster)
    {
        monster.GetModel().DisplayText("T_B_MISS");
    }
    public void DisplayDamageMonster(Monster monster, int damage)
    {
        monster.GetModel().PlayDamage(damage);
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
    public void UpdateAll(List<BattleCharacter> party, BattleCharacter selectedCharacter)
    {
        UpdateCharacterBars(party);
        SwapCharacterModel(selectedCharacter);
        UpdateSkills(selectedCharacter);
    }
    public void UpdateCharacterBars(List<BattleCharacter> party)
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
    public void SwapCharacterModel(BattleCharacter selectedCharacter)
    {
        characterModels.ShowCharacter(selectedCharacter.who);
    }
    public void HideCharacterModel()
    { 
        characterModels.FadeCharacter();
    }

    public void UpdateSkills(BattleCharacter selectedCharacter)
    {
        for (int i=0; i<5; i++)
        {
            if (i < selectedCharacter.skills.Count)
            {
                skillNode.GetChild<SkillBoxUI>(i).Update(selectedCharacter, selectedCharacter.skills[i]);
            }
            else
            {
                skillNode.GetChild<SkillBoxUI>(i).Empty();
            }
        }
    }
    public void ViewActionDetail(BattleSkill skill, int index)
    {
        lastViewedActionIndex = (skill.type == SkillType.BASIC) ? -2 : index;
        skillDesc.ShowSkill(skill);
    }

    public void HideActionDetail()
    {
        lastViewedActionIndex = -1;
        skillDesc.HideSkill();
        HideEstimateAll();
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
    public void ShowEstimate(Monster monster, int estimate)
    {
        monster.GetModel().ShowEstimate(estimate);
    }
    public void HideEstimateAll()
    {
        monsterNode.HideEstimateAll();
    }

    public void ShowCenterMessage(string text)
    {
        centerMessage.ShowText(text);
    }
    public void ShowCharacterMessage(string text)
    {
        characterMessage.ShowText(text);
    }

    public void SelectCharacter(BattleCharacter character, int index)
    {
        foreach (CharacterBar bar in partyNode.GetChildren().Cast<CharacterBar>())
        {
            bar.Deselect();
        }
        partyNode.GetChild<CharacterBar>(index).Select();

        // Update the currently viewed skill to the one of the new character.
        if (lastViewedActionIndex >= character.skills.Count)
        {
            HideActionDetail();
        }
        else if (lastViewedActionIndex >= 0)
        {
            ViewActionDetail(character.skills[lastViewedActionIndex], lastViewedActionIndex);
        }
    }
    public void ShakeStackCount(int index)
    {
        partyNode.GetChild<CharacterBar>(index).ShakeStack();
    }

    // Is handed an animation that wants to be played, and the party member's bar it should play on
    public void SpawnEffectParty(Node2D animation, BattleCharacter character)
    {
        AddChild(animation);
        characterModels.ShowCharacter(character.who);
        animation.Position = characterModels.Position - new Vector2(0, 400);
        characterModels.Show();

		foreach (CharacterBar bar in partyNode.GetChildren().Cast<CharacterBar>())
		{
			if (bar.Who == character.who)
			{
				bar.Update(character);
				bar.ShakeHP();
				break;
			}
		}
    }
}
