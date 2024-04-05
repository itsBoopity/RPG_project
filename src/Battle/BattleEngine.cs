using Godot;
using System;

/// <summary>
/// The main class and root scene facilitating battles.
/// </summary>
public partial class BattleEngine : Control
{
    public BattleUI Ui { get; private set; }
    private BattleFieldData bF;
    private BattleSfx sfx;
    private BattleSkill selectedSkill;
    private int selectedCharacter = -1;
    private CharacterRack party;
    private CharacterRack bench;
    private MonsterRack monsters;
    private CustomTimer timer;
    private float enemyTurnSpeed;

    private readonly BattleSkill[] basicSkills = { 
        new(new SkillBasicAttack()),
        new(new SkillBasicDefend()),
        new(new SkillBasicSwap()),
        new(new SkillBasicAnalyze()),
    };

    public override void _Ready()
    {
        Ui = GetNode<BattleUI>("%UI");
        party = GetNode<CharacterRack>("%Party");
        bench = GetNode<CharacterRack>("%Bench");
        monsters = GetNode<MonsterRack>("%Monsters");
        sfx = GetNode<BattleSfx>("%UiSfx");
        timer = GetNode<CustomTimer>("%TurnTimer");
        enemyTurnSpeed = Global.Settings.enemyTurnSpeed;
        bF = new BattleFieldData(party, bench, monsters);
    }

    public override void _Input(InputEvent @event)
    {
        if (state == ControlState.END_SCREEN)
        {
            if (@event.IsActionPressed("battle_clickTarget")) ExitBattle();
        }
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (state == ControlState.PLAYER_DEFAULT)
        {
            if (@event.IsActionPressed("battle_skill0")) SelectSkill(0);
            else if (@event.IsActionPressed("battle_skill1")) SelectSkill(1);
            else if (@event.IsActionPressed("battle_skill2")) SelectSkill(2);
            else if (@event.IsActionPressed("battle_skill3")) SelectSkill(3);
            else if (@event.IsActionPressed("battle_skill4")) SelectSkill(4);

            else if (@event.IsActionPressed("battle_basicskill0")) SelectBasicSkill(0);
            else if (@event.IsActionPressed("battle_basicskill1")) SelectBasicSkill(1);
            else if (@event.IsActionPressed("battle_basicskill2")) SelectBasicSkill(2);
            else if (@event.IsActionPressed("battle_basicskill3")) SelectBasicSkill(3);
        }
        else if (state == ControlState.END_SCREEN)
        {
            if (@event.IsActionPressed("ui_accept")) ExitBattle();
            return;
        }

        if (state != ControlState.FULLY_DISABLED)
        {
            if (@event.IsActionPressed("battle_character0")) SelectCharacter(0);
            else if (@event.IsActionPressed("battle_character1")) SelectCharacter(1);
            else if (@event.IsActionPressed("battle_character2")) SelectCharacter(2);
        }
        if (state != ControlState.FULLY_DISABLED && state != ControlState.ENEMY_TURN && state != ControlState.END_SCREEN)
        {
            if (@event.IsActionPressed("ui_cancel"))
            {
                if (state == ControlState.PLAYER_CUSTOMWINDOW)
                {
                    ExitCustomSkillWindowState(true);
                }
                else
                {
                    PlayerSwitchDefaultIfPossible();
                }
            }
        }
    }

    /// <summary>
    /// Loads all the data and calls UpdateUI() at the end
    /// </summary>
    /// <param name="battleSetup">The battle setup that contains enemy data, special effects and win conditions.</param>
    public void Initiate(BattleSetup battleSetup)
    {
        Ui.Reset();
        selectedCharacter = -1;
        party.Clear();
        bench.Clear();
        monsters.Clear();
        foreach (BattleCharacter character in GameData.Instance.GetBattleParty()) 
        {
            party.Add(character);
            StartInitializeCharacter(character);
            character.TookDamage += CharacterTookDamage;
            character.TookDamage += Ui.CharacterTookDamage;
        }
        foreach (BattleCharacter character in GameData.Instance.GetBattleBench()) 
        {
            bench.Add(character);
            StartInitializeCharacter(character);
            character.TookDamage += CharacterTookDamage;
            character.TookDamage += Ui.CharacterTookDamage;
        }
        foreach (PackedScene m in battleSetup.Monsters)
        {
            BattleMonster monster = m.Instantiate<BattleMonster>();
            StartInitializeMonster(monster);
            monsters.Add(monster);
        }
        SelectCharacter(0);
        PlayerTurn();
    }

    /// <summary>
    /// Setup BattleActor stats and effects at the start of battle; And connect signals to BattleEngine 
    /// </summary>
    private void StartInitializeCharacter(BattleCharacter character)
    {
        character.ChangeStack(character.Level);
        foreach (BattleSkill skill in character.Skills)
        {
            skill.Initialize();
        }
        character.StatsChanged += Ui.UpdateCharacterBar;
    }

    private void StartInitializeMonster(BattleMonster monster)
    {
        foreach (BattleSkill skill in monster.Skills)
        {
            skill.Initialize();
        }
        monster.Attacked += PlayerExecuteEnemyTargetAction;
        monster.Missed += PlayerMissEnemyTargetAction;
        monster.TookDamage += MonsterTookDamage;
        monster.Selected += PlayerExecuteEnemySelectAction;
        monster.PlayerAttackedVfx += Ui.PlayerAttackedVfx;
        monster.DisplayCenterMessage += Ui.PrintCenterMessage;
        monster.FinishedTurn += EnemyTurnPerformStep;
    }

    //Called upon finishing the fight. Performs cleanup and finalizing, such as saving character data and giving rewards
    private void Finish()
    {
        sfx.Victory();
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
        data.UpdatePartyLoadout(party.GetAllAsCharacterList(), bench.GetAllAsCharacterList());
        SwitchState(ControlState.END_SCREEN);
    }

    private void ExitBattle()
    {
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
                speedFactor = Math.Min((ratio - 1.0f) * 15.0f, 15.0f);
            }
            timer.CustomStart(15.0f + speedFactor);
        }
    }

    private void PlayerTurn()
    {
        // Generate spots for healing/buff/utility
        
        EnterPlayerDefaultState();

        foreach (BattleMonster monster in monsters.GetAll())
            monster.LoadUpcomingTurn(bF);
        
        foreach (BattleCharacter character in party.GetAll())
        {
            character.NextTurn();
        }

        Ui.UpdateAll(party, GetCurrentPartyMember());

        CalculateAndStartTimer();
    }

    private void OutOfTime()
    {
        Ui.PrintCenterMessage("Ran out of time!");
        Ui.HideActionDetail();
        GetNode<AudioStreamPlayer>("%TimeOutAudio").Play();
        EnemyTurn();
    }

    private async void EnemyTurn()
    {
        timer.CustomStop();
        SwitchState(ControlState.ENEMY_TURN, true);
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
        Ui.UpdateAll(party, GetCurrentPartyMember());
        PlayerTurn();
    }

    public void CharacterTookDamage(BattleCharacter character, BattleActor damageDealer, int damage)
    {
        if (character.Health <= 0)
        {
            // TODO: Gameover
            Ui.PrintCharacterMessage("You deaded.");
            GD.Print("Game over you deaded :(");
        }
    }
    public void MonsterTookDamage(BattleMonster monster, BattleActor damageDealer, int damage)
    {
        if (monster.Health <= 0)
        {
            Ui.PrintCenterMessage(String.Format(Tr("{0} defeated!"), monster.DisplayName));
            monster.Defeated(damageDealer);
            if (monsters.Count == 0)
            {
                CallDeferred(MethodName.Finish);
            }
        }
    }

    public void SelectCharacter(int index)
    {
        if (index >= party.Count) return;
        PlayerSwitchDefaultIfPossible();
        if (!GetPartyMember(index).TurnActive && state != ControlState.ENEMY_TURN)
        {
            Ui.PrintCharacterMessage("Character's turn already over.");
        }
        if (index == selectedCharacter) return;

        sfx.StrongClickPitchIndex(index);

        selectedCharacter = index;
        Ui.SelectCharacter(GetCurrentPartyMember(), index, bF);
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
        Ui.UpdateSkills(GetCurrentPartyMember());
    }
    private void UseUpTurn(BattleCharacter character)
    {
        character.TurnActive = false;
        Ui.UpdateSkills(character);
        if (!GetCurrentPartyMember().TurnActive)
        {
            Ui.HideCharacterModel();
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
