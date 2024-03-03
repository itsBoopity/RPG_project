using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// The main class and root scene facilitating battles.
/// </summary>
public partial class BattleEngine : Control
{
    private BattleUI ui;
    public BattleUI Ui { get {return ui;} }
    private BattleSfx sfx;
    private BattleSkill selectedSkill;
    private int selectedCharacter = -1;
    private List<BattleCharacter> party;
    private List<BattleCharacter> bench;
    private List<Monster> monsters;
    public CustomTimer timer;

    private ControlState state;
    private float enemyTurnSpeed;

    private readonly BattleSkill[] basicSkills = { 
        SkillDatabase.GetSkillData(SkillId.BasicAttack)
        };

    public override void _Ready()
    {
        ui = GetNode<BattleUI>("UI");
        timer = GetNode<CustomTimer>("TurnTimer");
        sfx = GetNode<BattleSfx>("BattleSfx");
        enemyTurnSpeed = Global.Settings.enemyTurnSpeed;
    }
    public override void _UnhandledKeyInput(InputEvent @event)
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

            else if (@event.IsActionPressed("battle_basicskill0")) SelectBasicSkill(0);
        }
        if (state != ControlState.ENEMY_TURN)
        {
            if (@event.IsActionPressed("ui_cancel")) ExitCurrentMode();
        }
    }

    /// <summary>
    /// Loads all the data and calls UpdateUI() at the end
    /// </summary>
    /// <param name="battleSetup">The battle setup that contains enemy data, special effects and win conditions.</param>
    public void Initiate(BattleSetup battleSetup)
    {
        selectedCharacter = -1;
        party = GameData.Instance.GetBattleParty();
        bench = GameData.Instance.GetBattleBench();
        foreach (BattleCharacter character in party)
        {
            StartInitializeCharacter(character);
        }
        foreach (BattleCharacter character in bench)
        {
            StartInitializeCharacter(character);
        }
        monsters = new List<Monster>();
        foreach (MonsterId id in battleSetup.MonsterIds)
        {
            Monster monster = MonsterFactory.Create(id);
            monsters.Add(monster);
            ui.AddMonster(monster.GetModel());
        }
        SelectCharacter(0);
        PlayerTurn();
    }

    /// <summary>
    /// Setup character stats and effects at the start of battle.
    /// </summary>
    public void StartInitializeCharacter(BattleCharacter character)
    {
        character.stack = character.level;
    }

    //Called upon finishing the fight. Performs cleanup and finalizing, such as saving character data and giving rewards
    private void Finish()
    {
        GD.Print("[UNFINISHED] Don't forget to code in adding of rewards. - BattleEngine.Finish()");
        timer.CustomStop();
        ui.ClearMonsters();
        GameData data = GameData.Instance;
        foreach (BattleCharacter i in party)
        {
            data.UpdateCharacter(i);
        }
        foreach (BattleCharacter i in bench)
        {
            data.UpdateCharacter(i);
        }
        GetNode<Global>("/root/Global").EndBattle();
    }

    // ---------------------- GETTERS --------------------------------------------------
    public BattleCharacter GetPartyMember(int index)
    {
        if (index >= party.Count)
        {
            throw new ArgumentException("BattleEngine.GetPartyMember index out of bounds.");
        }
        else
        {
            return party[index];
        }
    }

    public int GetPartySize()
    {
        return party.Count;
    }

    /// <summary>
    /// Returns the party member that is currently selected and being controlled.
    /// </summary>
    public BattleCharacter GetCurrentPartyMember()
    {
        return GetPartyMember(selectedCharacter);
    }

    // ---------------------- STATE LOGIC ----------------------------------------------
    /// <summary>
    /// Sets the ControlState and updates UI to reflect that.
    /// </summary>
    private void SetState(ControlState state)
    {
        this.state = state;
        ui.UpdateInfoLabel(state);
    }
    private void EnterPlayerDefault()
    {
        SetState(ControlState.PLAYER_DEFAULT);
        ui.ShowMostUI();
    }
    private void EnterTargetMode()
    {
        SetState(ControlState.PLAYER_TARGETTING_ENEMY);
        ui.ShowReticle();

        foreach (Monster monster in monsters)
        {
            monster.targettingEnabled = true;
            ui.ShowEstimate(monster, selectedSkill.EstimateDamage(this, GetCurrentPartyMember(), monster));
        }
        ui.HideMostUI();
    }
    private void EnterTargetAllyMode()
    {
        SetState(ControlState.PLAYER_TARGETTING_ALLY);
        // TODO: Hide party, model,skill, skillDesc UI and show estimated damage on enemies;
    }
    private void EnterEnemyTurn()
    {
        SetState(ControlState.ENEMY_TURN);  
        ui.HideMostUI();
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
            ui.DisableReticle();
            foreach (Monster i in monsters)
            {
                i.targettingEnabled = false;
            }
            ui.HideEstimateAll();
            ui.HideActionDetail();
            ui.ShowMostUI();
            // TODO: Show the party, model,skill, skillDesc UI again. Hide the enemy estimates;
        }
    }

    // ---------------------- GAME LOGIC --------------------------------------------------
    private void CalculateAndStartTimer()
    {
        if (Global.Settings.timerEnabled)
        {
            float partySpeed = 0.0f, enemySpeed = 1.0f;
            foreach (BattleActor i in party)
            {
                partySpeed += i.GetSpd();
            }
            foreach (BattleActor i in monsters)
            {
                enemySpeed += i.GetSpd();
            }
            float ratio = partySpeed/enemySpeed;
            float speedFactor = 0.0f;

            if (ratio < 1.0)
            {
                speedFactor = Math.Max(-5.0f, ratio * 10.0f - 10.0f);
            }
            else if (ratio > 1.0)
            {
                speedFactor = Math.Min((ratio - 1.0f) * 10.0f, 10.0f);
            }
            timer.CustomStart(10.0f + speedFactor);
        }
    }

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

        ui.UpdateAll(party, GetCurrentPartyMember());

        // Tick down cooldowns, statusi etc.

        CalculateAndStartTimer();
    }
    private async void EnemyTurn()
    {
        timer.CustomStop();
        ExitCurrentMode(); // First set the player mode and UI back to defaault
        EnterEnemyTurn(); // Then enter enemy Turn
        await ToSignal(GetTree().CreateTimer(0.25f / enemyTurnSpeed), SceneTreeTimer.SignalName.Timeout);

        ui.PlayTurnAnnouncement();
        foreach (Monster monster in monsters)
        {
            await ToSignal(GetTree().CreateTimer(0.85f / enemyTurnSpeed), SceneTreeTimer.SignalName.Timeout);
            await ToSignal(monster.ExecuteTurn(this), AnimationPlayer.SignalName.AnimationFinished);
        }
        await ToSignal(GetTree().CreateTimer(0.85f / enemyTurnSpeed), SceneTreeTimer.SignalName.Timeout);
        ui.StopTurnAnnouncement();
        ui.UpdateAll(party, GetCurrentPartyMember()); // TODO: This should instead be animated and updated by each skill
        PlayerTurn();
    }

    public void PlayerMissed(Monster target)
    { 
        UseUpTurn(GetCurrentPartyMember());
        ExitCurrentMode();
        ui.DisplayMissMonster(target);
    }

    public void DoDamage(int damage, BattleActor user, BattleActor target) // damage is FINALIZED, this is just the engine doing the exact amount stated
    {
        if (damage < 0) damage = 0;
        target.hp -= damage;

        if (target is Monster monster)
        {
            ui.UpdateMonsterModel(monster);
            ui.DisplayDamageMonster(monster, damage);
        }
        else
        {
            ui.DisplayDamageCharacter(damage);
        }

        if (target.hp <= 0)
        {
            BattleActorDefeated(target);
        }
    }

    private void BattleActorDefeated(BattleActor actor)
    {
        if (actor is Monster monster)
        {
            ui.ShowCenterMessage(monster.name + " defeated!");
            monsters.Remove(monster);
            monster.Free();
            
            if (monsters.Count == 0)
                CallDeferred(MethodName.Finish);
        }
        else if (actor is BattleCharacter)
        {
            ui.ShowCharacterMessage("You deaded.");
            GD.Print("Game over you deaded :(");
            // TODO: if target is Character, player ded, game over
        }
    }

    public void ViewSkill(int index)
    {
        if (index < GetCurrentPartyMember().skills.Count)
        {
            ViewAction(GetCurrentPartyMember().skills[index], index);
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
        ui.ViewActionDetail(action, index);
        foreach (Monster monster in monsters)
        {
            ui.ShowEstimate(monster, action.EstimateDamage(this, GetCurrentPartyMember(), monster));
        }
    }

    public void SelectSkill(int index)
    {
        if (index < GetCurrentPartyMember().skills.Count && GetCurrentPartyMember().turnActive)
        {
            SelectAction(GetCurrentPartyMember().skills[index], index);
        }
    }
    public void SelectBasicSkill(int index)
    {
        if (index < basicSkills.Length && GetCurrentPartyMember().turnActive)
        {
            SelectAction(basicSkills[index], index);
        }        
    }

    public void SelectAction(BattleSkill action, int index)
    {
        int isUsable = action.IsUsable(GetCurrentPartyMember());
        if (isUsable > 0)
        {
            sfx.ErrorSound();
            if (isUsable == 1)
            {
                ui.ShowCharacterMessage("Skill is in cooldown!");
            } 
            else if (isUsable == 2)
            {
                ui.ShakeStackCount(selectedCharacter);
                ui.ShowCharacterMessage("Not enough stacks!");
            }
            return;
        }
        sfx.RollClickPitchIndex(index);
        if (action.targetting == TargettingType.ENEMY_TARGET)
        {
            selectedSkill = action;
            ui.ViewActionDetail(action, index);
            EnterTargetMode();
        }
        else if (action.targetting == TargettingType.ALLY_TARGET)
        {
            selectedSkill = action;
            EnterTargetAllyMode();
        }
    }

    public void ExecuteSkill(Monster monster, float targetEfficiency)
    {
        selectedSkill.Use(this, GetCurrentPartyMember(), monster, targetEfficiency);

        Node2D temp = selectedSkill.GetAnimation();
        AddChild(temp);
        temp.GlobalPosition = GetGlobalMousePosition();

        if (!selectedSkill.snap)
        {
            UseUpTurn(GetCurrentPartyMember());
        }
        
        ui.UpdateSkills(GetCurrentPartyMember());
        ExitCurrentMode();
    }

    public void SelectCharacter(int index)
    {
        if (index >= party.Count) return;
        ExitCurrentMode();
        if (!GetPartyMember(index).turnActive && state != ControlState.ENEMY_TURN)
        {
            ui.ShowCharacterMessage("Character's turn already over.");
        }
        if (index == selectedCharacter) return;

        sfx.StrongClickPitchIndex(index);

        selectedCharacter = index;
        ui.SelectCharacter(GetCurrentPartyMember(), index);
        ui.UpdateSkills(GetCurrentPartyMember());
        if (state != ControlState.ENEMY_TURN)
        {
            if (GetCurrentPartyMember().turnActive)
            {
                ui.SwapCharacterModel(GetCurrentPartyMember());
            }
            else
            {
                ui.HideCharacterModel();
            }
        }
    }

    private void RefreshTurn(BattleCharacter character)
    {
        character.turnActive = true;
        ui.UpdateCharacterBars(party);
        ui.UpdateSkills(GetCurrentPartyMember());
    }
    private void UseUpTurn(BattleCharacter character)
    {
        character.turnActive = false;
        ui.UpdateCharacterBars(party);
        ui.HideCharacterModel();
        ui.UpdateSkills(GetCurrentPartyMember());
        if (PlayerTurnFinished() && monsters.Count > 0)
        {
            EnemyTurn();
        }
    }

    /// <summary>
    /// Checks if all of player's party members have finished acting this turn.
    /// </summary>
    /// <returns>True if all finished, false if not yet.</returns>
    private bool PlayerTurnFinished()
    {
        foreach (BattleCharacter i in party)
        {
            if (i.turnActive)
            {
                return false;
            }
        }
        return true;
    }
}
