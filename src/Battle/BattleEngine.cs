using Godot;
using System;
using System.Linq;

/// <summary>
/// The main class and root scene facilitating battles.
/// </summary>
public partial class BattleEngine : Control
{
    public BattleUI Ui { get; private set; }
    private BattleSfx sfx;
    private BattleSkill selectedSkill;
    private int selectedCharacter = -1;
    private CharacterRack party;
    private CharacterRack bench;
    private MonsterRack monsters;
    private CustomTimer timer;

    private ControlState state;
    private float enemyTurnSpeed;

    private readonly BattleSkill[] basicSkills = { 
        new(new SkillBasicAttack()),
        new(new SkillBasicDefend())
    };

    public override void _Ready()
    {
        Ui = GetNode<BattleUI>("%UI");
        party = GetNode<CharacterRack>("%Party");
        bench = GetNode<CharacterRack>("%Bench");
        monsters = GetNode<MonsterRack>("%Monsters");
        sfx = GetNode<BattleSfx>("%BattleSfx");
        timer = GetNode<CustomTimer>("%TurnTimer");
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
        party.Clear();
        bench.Clear();
        monsters.Clear();
        foreach (BattleCharacter character in GameData.Instance.GetBattleParty()) 
        {
            party.Add(character);
            StartInitializeActor(character);
            character.TookDamage += CharacterTookDamage;
            character.TookDamage += Ui.CharacterTookDamage;
        }
        foreach (BattleCharacter character in GameData.Instance.GetBattleBench()) 
        {
            bench.Add(character);
            StartInitializeActor(character);
            character.TookDamage += CharacterTookDamage;
            character.TookDamage += Ui.CharacterTookDamage;
        }
        foreach (PackedScene m in battleSetup.monsters)
        {
            BattleMonster monster = m.Instantiate<BattleMonster>();
            StartInitializeActor(monster);
            monsters.Add(monster);
            monster.Attacked += PlayerExecuteSelectedSkill;
            monster.Missed += PlayerMissSkill;
            monster.TookDamage += MonsterTookDamage;
            monster.PlayerAttackedVfx += Ui.PlayerAttackedVfx;
            monster.DisplayCenterMessage += Ui.ShowCenterMessage;
            monster.FinishedTurn += EnemyTurnPerformStep;
        }
        SelectCharacter(0);
        PlayerTurn();
    }

    /// <summary>
    /// Setup BattleActor stats and effects at the start of battle.
    /// </summary>
    private void StartInitializeActor(BattleActor actor)
    {
        actor.ChangeStack(actor.Level);
        foreach (BattleSkill skill in actor.Skills)
        {
            skill.Initialize();
        }
    }

    //Called upon finishing the fight. Performs cleanup and finalizing, such as saving character data and giving rewards
    private void Finish()
    {
        GD.Print("[UNFINISHED] Don't forget to code in adding of rewards. - BattleEngine.Finish()");
        timer.CustomStop();
        monsters.Clear();
        GameData data = GameData.Instance;
        foreach (BattleCharacter i in party.GetAll())
        {
            data.UpdateCharacterHealth(i.Who, i.Health);
        }
        foreach (BattleCharacter i in bench.GetAll())
        {
            data.UpdateCharacterHealth(i.Who, i.Health);
        }
        SceneManager.Instance.EndBattle();
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
            // TODO: Show the skill UI again, hide the bar blob areas;
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
    }

    // ---------------------- GAME LOGIC --------------------------------------------------
    private void CalculateAndStartTimer()
    {
        if (Global.Settings.timerEnabled)
        {
            float partySpeed = 0.0f, enemySpeed = 1.0f;
            foreach (BattleActor i in party.GetAll())
            {
                partySpeed += i.GetSpeed();
            }
            foreach (BattleActor i in monsters.GetAll())
            {
                enemySpeed += i.GetSpeed();
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
        
        EnterPlayerDefaultMode();

        BattleFieldData bf = new(party, bench, monsters);
        foreach (BattleMonster monster in monsters.GetAll())
            monster.LoadUpcomingTurn(bf);
        
        foreach (BattleCharacter character in party.GetAll())
        {
            character.NextTurn();
        }

        Ui.UpdateAll(party, GetCurrentPartyMember());

        CalculateAndStartTimer();
    }

    private async void EnemyTurn()
    {
        timer.CustomStop();
        ExitCurrentMode(); // First set the player mode and UI back to defaault
        EnterEnemyTurn(); // Then enter enemy Turn
        foreach (BattleMonster monster in monsters.GetAll())
        {
            monster.NextTurn();
        }
        await ToSignal(GetTree().CreateTimer(0.25f / enemyTurnSpeed), SceneTreeTimer.SignalName.Timeout);
        Ui.PlayTurnAnnouncement();
        EnemyTurnPerformStep();
    }

    private async void EnemyTurnPerformStep()
    {
        if (state != ControlState.ENEMY_TURN)
        {
            return;
        }

        BattleMonster monster = monsters.GetNextCanAct();
        await ToSignal(GetTree().CreateTimer(0.85f / enemyTurnSpeed), SceneTreeTimer.SignalName.Timeout);
        if (monster != null)
        {
            BattleFieldData data = new(party, bench, monsters);
            monster.ExecuteTurn(data);
        }
        else
        {
            EnemyTurnFinished();
        }
    }

    private void EnemyTurnFinished()
    {
        Ui.StopTurnAnnouncement();
        Ui.UpdateAll(party, GetCurrentPartyMember()); // TODO: This should instead be animated and updated by each skill
        PlayerTurn();
    }

    public void CharacterTookDamage(BattleCharacter character, BattleActor damageDealer, int damage)
    {
        if (character.Health <= 0)
        {
            // TODO: Gameover
            Ui.ShowCharacterMessage("You deaded.");
            GD.Print("Game over you deaded :(");
        }
    }
    public void MonsterTookDamage(BattleMonster monster, BattleActor damageDealer, int damage)
    {
        if (monster.Health <= 0)
        {
            Ui.ShowCenterMessage(String.Format(Tr("{0} defeated!"), monster.DisplayName));
            monster.Defeated(damageDealer);
            if (monsters.Count == 0)
            {
                CallDeferred(MethodName.Finish);
            }
        }
    }

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
        Ui.ViewActionDetail(action, index);
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
                Ui.ShowCharacterMessage("T_B_MSG_CD");
            } 
            else if (isUsable == SkillUsableResult.NOT_ENOUGH_STACKS)
            {
                Ui.ShakeStackCount(selectedCharacter);
                Ui.ShowCharacterMessage("T_B_MSG_NOSTACK");
            }
        }
        else
        {
            sfx.RollClickPitchIndex(index);
            if (action.Targetting == TargettingType.ENEMY_TARGET)
            {
                selectedSkill = action;
                Ui.ViewActionDetail(action, index);
                EnterTargetMode();
            }
            else if (action.Targetting == TargettingType.ALLY_TARGET)
            {
                selectedSkill = action;
                EnterTargetAllyMode();
            }
        }
        
    }

    private void PlayerExecuteSelectedSkill(BattleMonster monster, float appendageCoef)
    {
        BattleFieldData bf = new(party, bench, monsters);
        BattleInteractionData bInteraction = new(GetCurrentPartyMember(), monster, appendageCoef);
        selectedSkill.Use(bf, bInteraction);

        Ui.PlayVfx(selectedSkill.Animation, GetGlobalMousePosition());

        if (!selectedSkill.Snap)
        {
            UseUpTurn(GetCurrentPartyMember());
        }
        
        Ui.UpdateSkills(GetCurrentPartyMember());
        ExitCurrentMode();
    }

    public void PlayerMissSkill(BattleMonster monster)
    {
        selectedSkill.Miss(GetCurrentPartyMember(), monster);
        monster.PlayerMissed();
        UseUpTurn(GetCurrentPartyMember());
        ExitCurrentMode();
    }

    public void SelectCharacter(int index)
    {
        if (index >= party.Count) return;
        ExitCurrentMode();
        if (!GetPartyMember(index).TurnActive && state != ControlState.ENEMY_TURN)
        {
            Ui.ShowCharacterMessage("Character's turn already over.");
        }
        if (index == selectedCharacter) return;

        sfx.StrongClickPitchIndex(index);

        selectedCharacter = index;
        Ui.SelectCharacter(GetCurrentPartyMember(), index);
        Ui.UpdateSkills(GetCurrentPartyMember());
        if (state != ControlState.ENEMY_TURN)
        {
            if (GetCurrentPartyMember().TurnActive)
            {
                Ui.SwapCharacterModel(GetCurrentPartyMember());
            }
            else
            {
                Ui.HideCharacterModel();
            }
        }
    }

    private void RefreshTurn(BattleCharacter character)
    {
        character.TurnActive = true;
        Ui.UpdateCharacterBars(party);
        Ui.UpdateSkills(GetCurrentPartyMember());
    }
    private void UseUpTurn(BattleCharacter character)
    {
        character.TurnActive = false;
        Ui.UpdateCharacterBars(party);
        Ui.HideCharacterModel();
        Ui.UpdateSkills(GetCurrentPartyMember());
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
        foreach (BattleCharacter character in party.GetAll())
        {
            if (character.TurnActive)
            {
                return false;
            }
        }
        return true;
    }
}
