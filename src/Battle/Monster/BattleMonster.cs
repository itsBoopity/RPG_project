using System;
using Godot;

public partial class BattleMonster: BattleActor
{
    [Signal]
    public delegate void AttackedEventHandler(BattleMonster who, float appendageCoef);
    [Signal]
    public delegate void MissedEventHandler(BattleMonster who);
    [Signal]
    public delegate void SelectedEventHandler(BattleMonster who);
    [Signal]
    public delegate void FinishedTurnEventHandler();
    [Signal]
    new public delegate void TookDamageEventHandler(BattleMonster who, BattleActor damageDealer, int damage);

    [Signal]
    public delegate void PlayerAttackedVfxEventHandler(AnimatedSpriteOneOff vfx, BattleCharacter attackedCharacter);
    [Signal]
    public delegate void DisplayCenterMessageEventHandler(string message);

    protected MonsterVisuals visuals;
    private MonsterTargettingEnabled targettingEnabled = MonsterTargettingEnabled.DISABLED;
    
    //Position of party member and skill index that is going to be used, decided when player turn starts
    public int PartyTarget { get; protected set; }
    public BattleSkill LoadedSkill { get; protected set; }
    public bool IsDisappearing { get; private set; }
    public MonsterId Id { get { return Stats.Id; } }

    public string AnalysisData { get { return Stats.AnalysisInfo; } }
    
    new private MonsterStats Stats { get {return (MonsterStats)base.Stats; }}

    public BattleMonster() {}

    public override void _Ready()
    {
        visuals = GetNode<MonsterVisuals>("%Visuals");
        visuals.UpdateHP(Health, MaxHealth);
        Stats.Visuals = visuals;
        Stats.SignalHitResult += GetHitResult;
        Stats.SignalIntent +=  SetIntent;
        Stats.Initialize();
    }

    public void SetTargettingAttack()
    {
        targettingEnabled = MonsterTargettingEnabled.ENABLED_ATTACK;
    }
    public void SetTargettingSelect()
    {
        targettingEnabled = MonsterTargettingEnabled.ENABLED_SELECT;
    }
    public void DisableTargetting()
    {
        targettingEnabled = MonsterTargettingEnabled.DISABLED;
    }

    public async void Defeated(BattleActor byWho)
    {
        targettingEnabled = MonsterTargettingEnabled.DISABLED;
        IsDisappearing = true;
        Stats.OnDefeated(byWho);
        await ToSignal(visuals.AnimateDefeat(), AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
    }

    public bool CanAct()
    {
        return !IsDisappearing && TurnActive;
    }

    public void LoadUpcomingTurn(BattleFieldData bF)
    {
        Stats.LoadUpcomingTurn(bF);
    }

    public async void ExecuteTurn(BattleFieldData bF)
    {
        if (LoadedSkill != null)
        {
            BattleInteractionData bI;
            if (LoadedSkill.IsAoE)
            {
                bI = new BattleInteractionData(this, bF.party.GetAllAsActorList()).AddFloatValue(1.0f);
                EmitSignal( SignalName.DisplayCenterMessage,
                        String.Format(Tr("T_BM_MONSTERATTACK_MULTI"), DisplayName, LoadedSkill.DisplayName));
            }
            else
            {
                bI = new BattleInteractionData(this, bF.party[PartyTarget]).AddFloatValue(1.0f);
                EmitSignal( SignalName.DisplayCenterMessage,
                        String.Format(Tr("T_BM_MONSTERATTACK_SINGLE"), DisplayName, LoadedSkill.DisplayName, bF.party[PartyTarget].DisplayName));
            }

            LoadedSkill.Use(bF, bI);
            AnimatedSpriteOneOff animation = LoadedSkill.Animation;
            EmitSignal(SignalName.PlayerAttackedVfx, animation, bF.party[PartyTarget]);
            await ToSignal(animation, AnimatedSpriteOneOff.SignalName.AnimationFinished);
        }
        TurnActive = false;
        EmitSignal(SignalName.FinishedTurn);
    }

    public void AppendageHitCheck(MonsterAppendage target)
    {
        if (targettingEnabled == MonsterTargettingEnabled.ENABLED_ATTACK)
        {
            Stats.AppendageHit(target);
            GetViewport().SetInputAsHandled();
        }
        else if (targettingEnabled == MonsterTargettingEnabled.ENABLED_SELECT)
        {
            EmitSignal(SignalName.Selected, this);
            GetViewport().SetInputAsHandled();
        }
    }

    public void AppendageMissCheck()
    {
        if (targettingEnabled  == MonsterTargettingEnabled.ENABLED_ATTACK)
        {
            AppendageMiss();
            GetViewport().SetInputAsHandled();
        } 
        else if (targettingEnabled == MonsterTargettingEnabled.ENABLED_SELECT)
        {
            EmitSignal(SignalName.Selected, this);
            GetViewport().SetInputAsHandled();
        }
        
    }

    public override void SustainDamage(BattleActor damageDealer, int finalDamage)
    {
        base.SustainDamage(damageDealer, finalDamage);
        visuals.UpdateHP(Health, MaxHealth);
        visuals.PlayDamage(finalDamage);
    }

    private void AppendageMiss()
    {
        EmitSignal(SignalName.Missed, this);
    }

    private void GetHitResult(float appendageCoef)
    {
        EmitSignal(SignalName.Attacked, this, appendageCoef);
    }

    /// <summary>
    /// Set actions that are to be performed on this BattleMonster's turn.
    /// Attacks party member based on position in party.
    /// </summary>
    /// <param name="targetIndex"></param>
    /// <param name="skillIndex"></param>
    private void SetIntent(int targetIndex, int skillIndex)
    {
        PartyTarget = targetIndex;
        if (skillIndex >= 0)
        {
            LoadedSkill = Skills[skillIndex];
        }
        else
        {
            LoadedSkill = null;
        }
    }
}