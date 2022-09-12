using Godot;
using System.Collections.Generic;

public class BattleEngine : Node2D
{
    private ControlState state;
    private int selectedCharacter = -1;
    public List<Character> party;
    public List<Character> bench;
    public List<Monster> monsters;
    public BattleTimer timer;

    private Node partyNode;
    private MonsterRack monsterNode;
    private CanvasItem skillNode;
    private CharacterModelRack characterModels;
    private RichTextLabel infoLabel;
    private SkillDescription skillDesc;

    public override void _Ready()
    {
        timer = GetNode<BattleTimer>("%Timer");
        partyNode = GetNode("%PartyBar");
        characterModels = GetNode<CharacterModelRack>("CharacterModels");
        monsterNode = GetNode<MonsterRack>("%Monsters");
        skillNode = GetNode<CanvasItem>("%Skills");
        infoLabel = GetNode<RichTextLabel>("%InfoLabel");
        skillDesc = GetNode<SkillDescription>("%SkillDescription");
    }

    //Loads all the data and calls UpdateUI() at the end
    public void Initiate(BattleSetup battleSetup)
    {
        GameData data = Global.data;
        selectedCharacter = -1;

        party = new List<Character>();
        bench = new List<Character>();
        foreach (CharacterEnum i in data.party)
            if (data.GetCharacter(i) != null)
                party.Add(data.GetCharacter(i).Clone());
        foreach (CharacterEnum i in data.bench)
                bench.Add(data.GetCharacter(i).Clone());
        
        monsters = new List<Monster>();
        foreach (int i in battleSetup.monsterID)
            monsters.Add(MonsterFactory.Create(i));
        foreach (Monster monster in monsters)
            monsterNode.AddModel(monster.GetModel());
        
        SelectCharacter(0);
        UpdateUI();
        PlayerTurn();
    }

    //Called upon finishing the fight. Performs cleanup and finalizing, such as saving character data and giving rewards
    private void Finish()
    {
        monsterNode.Clear();

        GameData data = Global.data;
        foreach (Character i in party)
            data.UpdateCharacter(i.Reset());
        foreach (Character i in bench)
            data.UpdateCharacter(i.Reset());

        //Add rewards
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
            Finish();
        if (state != ControlState.FULLY_DISABLED)
        {
            if (@event.IsActionPressed("battle_character1")) SelectCharacter(0);
            else if (@event.IsActionPressed("battle_character2")) SelectCharacter(1);
            else if (@event.IsActionPressed("battle_character3")) SelectCharacter(2);
        }
        if (state == ControlState.PLAYER_DEFAULT)
        {
            if (@event.IsActionPressed("battle_skill1")) SelectSkill(0);
            else if (@event.IsActionPressed("battle_skill2")) SelectSkill(1);
            else if (@event.IsActionPressed("battle_skill3")) SelectSkill(2);
            else if (@event.IsActionPressed("battle_skill4")) SelectSkill(3);
            else if (@event.IsActionPressed("battle_skill5")) SelectSkill(4);
        }
        if (state != ControlState.ENEMY_TURN)
            if (@event.IsActionPressed("ui_cancel")) ExitCurrentMode();
    }

    public void SetTimer()
    {
        if (Global.settings.timerEnabled)
        {
            float partySpeed = 0, enemySpeed = 1;
            foreach (BattleFigure i in party)
                partySpeed += i.SPD;
            foreach (BattleFigure i in monsters)
                enemySpeed += i.SPD;

            float ratio = partySpeed/enemySpeed;
            int coefficient = 0;

            if (ratio < 0.5)
                coefficient = -5;
            else if (ratio < 1)
                coefficient = Godot.Mathf.RoundToInt(ratio * 10) - 10;
            else if (ratio > 2)
                coefficient = 10;
            else if (ratio > 1)
                coefficient = Godot.Mathf.RoundToInt((ratio - 1) * 10);
            
            timer.StartTimer(10 + coefficient);
        }
    }
    private void PlayerTurn()
    {
        // Generate spots for healing/buff/utility
        EnablePlayerControl();

        infoLabel.BbcodeText = "Player Turn";
        foreach (Monster monster in monsters)
            monster.LoadUpcomingTurn(this);
        
        foreach (Character character in party)
            character.turnActive = true;

        // Tick down cooldowns, statusi etc.

        SetTimer();
    }

    private void EnemyTurn()
    {
        DisablePlayerControl();
        ExitCurrentMode();
        state = ControlState.ENEMY_TURN;

        infoLabel.BbcodeText = "Enemy Turn";
        foreach (Monster monster in monsters)
            monster.ExecuteTurn(this);

        UpdateUI(); // This should instead be animated and updated by each skill
        PlayerTurn();
    }

    private void SelectSkill(int index)
    {
        if (index >= party[selectedCharacter].skills.Count) return;

        BattleSkill skill = party[selectedCharacter].skills[index];
        if (skill.type == SkillType.OFFENSIVE)
            ExecuteOffensive(party[selectedCharacter].skills[index]);
    }
    private void ExecuteOffensive(BattleSkill skill) { EnterTargetMode(skill); }

    public void ShowSkillDetail(int index)
    {
        if (index >= party[selectedCharacter].skills.Count) return;
        skillDesc.ShowSkill(party[selectedCharacter].skills[index], index);
    }
    private void HideSkillDetail()
    {
        skillDesc.Hide();
    }

    private void SelectCharacter(int index)
    {
        if (index >= party.Count) return;

        ExitCurrentMode();
        if (index == selectedCharacter) return;

        selectedCharacter = index;
        foreach (CharacterBar bar in partyNode.GetChildren())
            bar.Unselect();
        partyNode.GetChild<CharacterBar>(index).Select();

        if (skillDesc.Visible)
            ShowSkillDetail(skillDesc.currentFocus);

        UpdateUIModel();
        UpdateUISkill();
    }

    private void EnterTargetMode(BattleSkill skill)
    {
        infoLabel.BbcodeText = "Enemy Targetting";
        state = ControlState.PLAYER_TARGETTING_ENEMY;

        // Hide all the UI that should be hidden and show estimated damage on enemies;
    }
    private void EnterTargetSelfMode(BattleSkill skill)
    {
        infoLabel.BbcodeText = "Ally Targetting";
        state = ControlState.PLAYER_TARGETTING_SELF;
        
        // Hide party, model,skill, skillDesc UI and show estimated damage on enemies;
    }

    // <summary> 
    // Exits the player out of whatever mode they're in and sets them back to PLAYER_TURN (if currently in a state of control)
    //</summary>
    private void ExitCurrentMode()
    {
        if (state == ControlState.PLAYER_TARGETTING_SELF)
        {
            state = ControlState.PLAYER_DEFAULT;
            infoLabel.BbcodeText = "Player Turn";
            
            //Show the skill UI again, hide the bar blob areas;
        }
        else if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            state = ControlState.PLAYER_DEFAULT;
            infoLabel.BbcodeText = "Player Turn";
            
            //Show the party, model,skill, skillDesc UI again. Hide the enemy estimates;
        }
    }

    private void EnablePlayerControl()
    {
        skillNode.Show();
        state = ControlState.PLAYER_DEFAULT;
    }
    private void DisablePlayerControl()
    {
        skillNode.Hide();
        state = ControlState.ENEMY_TURN;
    }


    //Updates all UI using each of the standalone UpdateUI
    private void UpdateUI()
    {
        UpdateUICharacterBar();    
        UpdateUIModel();
        UpdateUISkill();
    }
    private void UpdateUICharacterBar()
    {
        for (int i=0; i<3; i++)
        {
            if (i < party.Count)
                partyNode.GetChild<CharacterBar>(i).Update(party[i]);
            else
                partyNode.GetChild<CharacterBar>(i).Hide();
        }
    }
    private void UpdateUIModel() { characterModels.ShowCharacter(party[selectedCharacter].who); }
    private void UpdateUISkill()
    {
        for (int i=0; i<5; i++)
        {
            if (i < party[selectedCharacter].skills.Count)
                skillNode.GetChild<SkillBoxUI>(i).Update(party[selectedCharacter].skills[i]);
            else
                skillNode.GetChild<SkillBoxUI>(i).Update(null);
        }
    }
}
