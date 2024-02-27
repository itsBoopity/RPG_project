using Godot;
using System.Collections.Generic;

public partial class BattleEngine : Control
{
    private int selectedCharacter = -1;
    public List<BattleCharacter> party;
    public List<BattleCharacter> bench;
    public List<Monster> monsters;
    public BattleTimer timer;
    private Timer internalTimer;

    // Separate the UI into another class you maniac
    private ControlState state;
    private Reticle reticle;
    private Node partyNode;
    private MonsterRack monsterNode;
    private CanvasItem skillNode;
    private CanvasItem basicSkillNode;
    private CharacterModelRack characterModels;
    private InfoLabel infoLabel;
    private SkillDescription skillDesc;

    public BattleNarration narration;
    public BattleNarration centerNarration;

    private BattleSkill selectedSkill;

    private TurnAnnouncement turnAnnouncement;
    private DamageCounter characterDamageCounter;


    private AnimationPlayer animationPlayer;
    private float enemyTurnSpeed;

    private Node sfx;

    private readonly BattleSkill[] basicSkills = { new BasicAttack() };

    public override void _EnterTree()
    {
        animationPlayer ??= GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Stop();
        animationPlayer.Play("OnEnter");
    }

    public override void _Ready()
    {
        reticle = GetNode<Reticle>("%Reticle");
        timer = GetNode<BattleTimer>("%Timer");
        internalTimer = GetNode<Timer>("InternalTimer");
        partyNode = GetNode("%PartyBar");
        characterModels = GetNode<CharacterModelRack>("CharacterModels");
        monsterNode = GetNode<MonsterRack>("%Monsters");
        skillNode = GetNode<CanvasItem>("%Skills");
        basicSkillNode = GetNode<CanvasItem>("%BasicSkills");
        infoLabel = GetNode<InfoLabel>("%InfoLabel");
        skillDesc = GetNode<SkillDescription>("%SkillDescription");
        narration = GetNode<BattleNarration>("BattleNarration");
        centerNarration = GetNode<BattleNarration>("CenterNarration");
        turnAnnouncement = GetNode<TurnAnnouncement>("TurnAnnouncement");
        characterDamageCounter = GetNode<DamageCounter>("CharacterDamageCounter");
        sfx = GetNode<Node>("Sfx");

        enemyTurnSpeed = Global.Settings.enemyTurnSpeed;
    }
    public override void _Input(InputEvent @event)
    {
        if (state != ControlState.FULLY_DISABLED)
        {
            if (@event.IsActionPressed("battle_character1")) SelectCharacter(0);
            else if (@event.IsActionPressed("battle_character2")) SelectCharacter(1);
            else if (@event.IsActionPressed("battle_character3")) SelectCharacter(2);
        }
        if (state == ControlState.PLAYER_DEFAULT)
        {
            if (@event.IsActionPressed("battle_skill0")) SelectSkill(0);
            else if (@event.IsActionPressed("battle_skill1")) SelectSkill(1);
            else if (@event.IsActionPressed("battle_skill2")) SelectSkill(2);
            else if (@event.IsActionPressed("battle_skill3")) SelectSkill(3);
            else if (@event.IsActionPressed("battle_skill4")) SelectSkill(4);

            else if (@event.IsActionPressed("battle_skill10")) SelectSkill(10);
        }
        if (state != ControlState.ENEMY_TURN)
        {
            if (@event.IsActionPressed("ui_cancel")) ExitCurrentMode();
        }
    }
    //Loads all the data and calls UpdateUI() at the end
    public void Initiate(BattleSetup battleSetup)
    {
        GameData data = Global.Data;

        selectedCharacter = -1;

        party = new List<BattleCharacter>();
        bench = new List<BattleCharacter>();
        foreach (CharacterEnum i in data.party)
        {
            if (data.GetCharacter(i) != null)
                party.Add(data.GetCharacter(i).Clone());
        }
        foreach (CharacterEnum i in data.bench)
        {
            bench.Add(data.GetCharacter(i).Clone());
        }
        monsters = new List<Monster>();
        foreach (int i in battleSetup.monsterID)
        {
            monsters.Add(MonsterFactory.Create(i));
        }
        foreach (Monster monster in monsters)
        {
            monster.Initiate(this);
            monsterNode.AddModel(monster.GetModel());
            monster.UpdateUIModel();
        }

        SelectCharacter(0);
        PlayerTurn();
    }

    //Called upon finishing the fight. Performs cleanup and finalizing, such as saving character data and giving rewards
    private void Finish()
    {
        GD.Print("[UNFINISHED] Don't forget to code in adding of rewards. - BattleEngine.Finish()");
        timer.Stop();
        monsterNode.Clear();
        GameData data = Global.Data;
        foreach (BattleCharacter i in party)
        {
            data.UpdateCharacter(i.Reset());
        }
        foreach (BattleCharacter i in bench)
        {
            data.UpdateCharacter(i.Reset());
        }
        GetNode<Global>("/root/Global").EndBattle();
    }
    private void SetTimer()
    {
        if (Global.Settings.timerEnabled)
        {
            float partySpeed = 0, enemySpeed = 1;
            foreach (BattleActor i in party)
            {
                partySpeed += i.GetSpd();
            }
            foreach (BattleActor i in monsters)
            {
                enemySpeed += i.GetSpd();
            }
            float ratio = partySpeed/enemySpeed;
            int coefficient = 0;

            if (ratio < 0.5)
            {
                coefficient = -5;
            }
            else if (ratio < 1)
            {
                coefficient = Godot.Mathf.RoundToInt(ratio * 10) - 10;
            }
            else if (ratio > 2)
            {
                coefficient = 10;
            }
            else if (ratio > 1)
            {
                coefficient = Godot.Mathf.RoundToInt((ratio - 1) * 10);
            }
            timer.StartTimer(10 + coefficient);
        }
    }

    // ---------------------- STATE LOGIC ----------------------------------------------
    // --------------------------------------------------------------------------------

    // Sets the state and updates the infolabel to show the appropriate state
    private void SetState(ControlState state)
    {
        this.state = state;
        infoLabel.UpdateUI(state);
    }
    private void EnterPlayerDefault()
    {
        SetState(ControlState.PLAYER_DEFAULT);
        ShowMostUI();
    }
    private void EnterTargetMode()
    {
        SetState(ControlState.PLAYER_TARGETTING_ENEMY);
        reticle.ShowReticle();

        foreach (Monster i in monsters)
        {
            i.targettingEnabled = true;
        }
        foreach (Monster i in monsters)
        {
            i.ShowEstimate(selectedSkill.EstimateDamage(this, party[selectedCharacter], i));
        }
        HideMostUI();
    }
    private void EnterTargetAllyMode()
    {
        SetState(ControlState.PLAYER_TARGETTING_ALLY);
        // TODO: Hide party, model,skill, skillDesc UI and show estimated damage on enemies;
    }
    private void EnterEnemyTurn()
    {
        SetState(ControlState.ENEMY_TURN);  
        HideMostUI();
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
            // TODO: Show the skill UI again, hide the bar blob areas;
        }
        else if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            SetState(ControlState.PLAYER_DEFAULT);
            reticle.Disable();
            foreach (Monster i in monsters)
            {
                i.targettingEnabled = false;
                i.HideEstimate();
            }

            HideSkillDetail();
            ShowMostUI();
            // TODO: Show the party, model,skill, skillDesc UI again. Hide the enemy estimates;
        }
    }
    // ---------------------- END OF STATE LOGIC ------------------------------------------

    // ---------------------- GAME LOGIC --------------------------------------------------
    private void PlayerTurn()
    {
        // Generate spots for healing/buff/utility
        EnterPlayerDefault();

        foreach (Monster monster in monsters)
            monster.LoadUpcomingTurn(this);
        
        foreach (BattleCharacter character in party)
        {
            character.turnActive = true;
            foreach (BattleSkill skill in character.skills)
                if (skill.currentCooldown > 0) skill.currentCooldown -= 1;
        }

        UpdateUI();

        // Tick down cooldowns, statusi etc.

        SetTimer();
    }
    private async void EnemyTurn()
    {
        timer.Stop();
        ExitCurrentMode(); // First set the player mode and UI back to defaault
        EnterEnemyTurn(); // Then enter enemy Turn

        internalTimer.Start(0.25f / enemyTurnSpeed);
        await ToSignal(internalTimer, "timeout");

        turnAnnouncement.Play();
        
        foreach (Monster monster in monsters)
        {
            internalTimer.Start(0.85f / enemyTurnSpeed);
            await ToSignal(internalTimer, "timeout");
            await ToSignal(monster.ExecuteTurn(this), "animation_finished");
        }

        turnAnnouncement.Stop();

        internalTimer.Start(0.85f/ enemyTurnSpeed);
        await ToSignal(internalTimer, "timeout");

        UpdateUI(); // TODO: This should instead be animated and updated by each skill
        PlayerTurn();
    }

    public void PlayerMissed(Monster target)
    { 
        UseUpTurn(party[selectedCharacter]);
        ExitCurrentMode();
        target.PlayDamage("Miss");
        // TODO: Play miss animation and sfx   
    }

    public void DoDamage(int damage, BattleActor user, BattleActor target) // damage is FINALIZED, this is just the engine doing the exact amount stated
    {
        if (damage < 0) damage = 0;
        target.hp -= damage;

        if (target is Monster)
        {
            ((Monster)target).UpdateUIModel();
            ((Monster)target).PlayDamage(damage);
        }
        else
        { characterDamageCounter.Play(damage); }
        if (target.hp <= 0)
        {
            BattleActorDefeated(target);
        }
    }

    private void BattleActorDefeated(BattleActor actor)
    {
        if (actor is Monster monster)
        {
            centerNarration.ShowText(monster.name + " defeated!");
            monsters.Remove(monster);
            monster.Free();
            
            if (monsters.Count == 0)
                CallDeferred(MethodName.Finish);
        }
        else if (actor is BattleCharacter)
        {
            narration.ShowText("You deaded.");
            GD.Print("Game over you deaded :(");
            // TODO: if target is Character, player ded, game over
        }
    }

    public void SelectSkill(int index)
    {
        if ( (index >= party[selectedCharacter].skills.Count && index < 10) ||
             !party[selectedCharacter].turnActive)
        {
            return;
        }

        sfx.GetNode<AudioStreamPlayer>("SkillClick").Play();

        BattleSkill skill;
        if (index > 9)
        {
            skill = basicSkills[index - 10];
        }
        else
            skill = party[selectedCharacter].skills[index];


        int isUsable = skill.IsUsable(party[selectedCharacter]);
        if (isUsable > 0)
        {
            sfx.GetNode<AudioStreamPlayer>("Error").Play();
            if (isUsable == 1) narration.ShowText("Skill is in cooldown!");
            else if (isUsable == 2)
            {
                partyNode.GetChild<CharacterBar>(selectedCharacter).ShakeStack();
                narration.ShowText("Not enough stacks!");
            }
            return;
        }

        if (skill.targetting == TargettingType.ENEMY_TARGET)
        {
            selectedSkill = skill;
            skillDesc.ShowSkill(skill, index);
            EnterTargetMode();
        }
        else if (skill.targetting == TargettingType.ALLY_TARGET)
        {
            selectedSkill = skill;
            EnterTargetAllyMode();
        }
    }

    public void ExecuteSkill(Monster monster, float targetEfficiency)
    {
        selectedSkill.Use(this, party[selectedCharacter], monster, targetEfficiency);

        Node2D temp = selectedSkill.GetAnimation();
        AddChild(temp);
        temp.GlobalPosition = GetGlobalMousePosition();

        if (!selectedSkill.snap)
        {
            UseUpTurn(party[selectedCharacter]);
        }
        
        UpdateUISkill();
        ExitCurrentMode();
    }

    public void SelectCharacter(int index)
    {
        if (index >= party.Count) return;
        ExitCurrentMode();
        if (!party[index].turnActive && state != ControlState.ENEMY_TURN)
        {
            narration.ShowText("Character's turn already over.");
        }
        if (index == selectedCharacter) return;

        sfx.GetNode<AudioStreamPlayer>("PartyBar").Play();

        selectedCharacter = index;
        foreach (CharacterBar bar in partyNode.GetChildren())
        {
            bar.Deselect();
        }
        partyNode.GetChild<CharacterBar>(index).Select();

        ShowSkillDetail(skillDesc.currentFocus);

        if (state != ControlState.ENEMY_TURN) UpdateUIModel();
        UpdateUISkill();
    }

    private void RefreshTurn(BattleCharacter character)
    {
        character.turnActive = true;
        UpdateUICharacterBar();
        UpdateUISkill();
    }
    private void UseUpTurn(BattleCharacter character)
    {
        character.turnActive = false;
        UpdateUICharacterBar();
        UpdateUIModel();
        UpdateUISkill();

        CheckForTurns();
    }

    // Checks if player still has turns, if not enemy turn time
    private void CheckForTurns()
    {
        foreach (BattleCharacter i in party)
        {
            if (i.turnActive) return;
        }
        if (monsters.Count != 0)
        {
            EnemyTurn();
        }
    }

    // ------------------------- UI UPDATES -------------------------------------------------------------
    // --------------------------------------------------------------------------------------------------

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
            {
                partyNode.GetChild<CharacterBar>(i).Update(party[i]);
            }
            else
            {
                partyNode.GetChild<CharacterBar>(i).Hide();
            }
        }
    }
    private void UpdateUIModel()
    { 
        if (!party[selectedCharacter].turnActive)
        {
            characterModels.FadeCharacter();
        }
        else
        {
            characterModels.ShowCharacter(party[selectedCharacter].who);
        }
    }
    private void UpdateUISkill()
    {
        for (int i=0; i<5; i++)
        {
            if (i < party[selectedCharacter].skills.Count)
            {
                skillNode.GetChild<SkillBoxUI>(i).Update(party[selectedCharacter], party[selectedCharacter].skills[i]);
            }
            else
            {
                skillNode.GetChild<SkillBoxUI>(i).Empty();
            }
        }

        for (int i=10; i<11; i++)
        {
            basicSkillNode.GetChild<SkillBoxUI>(i - 10).Update(party[selectedCharacter], basicSkills[i-10]);
        }
    }
    public void ShowSkillDetail(int index)
    {
        if (index == -1 || (index >= party[selectedCharacter].skills.Count && index < 10))
        {
            skillDesc.currentFocus = index;
            skillDesc.Hide();
            return;
        }
        BattleSkill skill;
        if (index > 9)
        {
            skill = basicSkills[index - 10];
        }
        else
        {
            skill = party[selectedCharacter].skills[index];
        }
        skillDesc.ShowSkill(skill, index);
        foreach (Monster i in monsters)
        {
            i.ShowEstimate(skill.EstimateDamage(this, party[selectedCharacter], i));
        }
    }

    private void HideMostUI()
    {
        characterModels.Hide();
        skillNode.Hide();
        basicSkillNode.Hide();
    }
    private void ShowMostUI()
    {
        characterModels.Show();
        skillNode.Show();
        basicSkillNode.Show();
    }


    public void HideSkillDetail()
    {
        skillDesc.HideSkill();
        foreach (Monster i in monsters)
        {
            i.HideEstimate();
        }
    }

    // Is handed an animation that wants to be played, and the party member's bar it should play on
    public void SpawnEffectParty(Node2D animation, int index)
    {
        AddChild(animation);
        characterModels.ShowCharacter(party[index].who);
        animation.Position = characterModels.Position - new Vector2(0, 400);
        characterModels.Show();

        partyNode.GetChild<CharacterBar>(index).Update(party[index]);
        partyNode.GetChild<CharacterBar>(index).ShakeHP();
    }
}
