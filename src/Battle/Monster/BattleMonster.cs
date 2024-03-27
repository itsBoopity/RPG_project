using System;
using Godot;

public partial class BattleMonster: BattleActor
{
    [Signal]
    public delegate void AttackedEventHandler(BattleMonster who, float appendageCoef);
    [Signal]
    public delegate void MissedEventHandler(BattleMonster who);
    [Signal]
    public delegate void FinishedTurnEventHandler();
    [Signal]
    new public delegate void TookDamageEventHandler(BattleMonster who, BattleActor damageDealer, int damage);

    [Signal]
    public delegate void PlayerAttackedVfxEventHandler(AnimatedSpriteOneOff vfx, BattleCharacter attackedCharacter);
    [Signal]
    public delegate void DisplayCenterMessageEventHandler(string message);

    public bool targettingEnabled = false;
    protected MonsterVisuals visuals;
    //Position of party member and skill index that is going to be used, decided when player turn starts
    public int PartyTarget { get; protected set; }
    public BattleSkill LoadedSkill { get; protected set; }
    public bool IsDisappearing { get; private set; }

    
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

    public async void Defeated(BattleActor byWho)
    {
        targettingEnabled = false;
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
            BattleInteractionData bI = new(this, bF.party[PartyTarget], 1.0f);
            LoadedSkill.Use(bF, bI);
            EmitSignal( SignalName.DisplayCenterMessage,
                        String.Format(Tr("{0} uses {1} on {2}!"), DisplayName, LoadedSkill.DisplayName, bF.party[PartyTarget].DisplayName));
            AnimatedSpriteOneOff animation = LoadedSkill.Animation;
            EmitSignal(SignalName.PlayerAttackedVfx, animation, bF.party[PartyTarget]);
            await ToSignal(animation, AnimatedSpriteOneOff.SignalName.AnimationFinished);
        }
        TurnActive = false;
        EmitSignal(SignalName.FinishedTurn);
    }

    public void AppendageHitCheck(MonsterAppendage target)
    {
        if (targettingEnabled)
        {
            Stats.AppendageHit(target);
        }
    }

    public void AppendageMissCheck()
    {
        if (targettingEnabled)
        {
            AppendageMiss();
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