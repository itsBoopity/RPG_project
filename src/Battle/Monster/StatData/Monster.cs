using Godot;

public abstract class Monster: BattleActor
{ 
    BattleEngine battleEngine = null;

    public bool targettingEnabled = false; // Whether clicks on monsters should be read;
    protected MonsterModel model = null;

    // The default damage modifier that gets used when non targetting offensive skills are used 
    protected float defaultDamage = 0.5f;
    
    //Position of party member and skill index that is going to be used, decided when player turn starts
    protected int targetCharacter = 0;
    protected int targetSkill = 0;

    public Monster()
    {
        model = GD.Load<PackedScene>("res://Objects/Monster/" + this.GetType().Name + ".tscn").Instantiate<MonsterModel>();
        model.SetOwner(this);
    }

    public void Free()
    {
        targettingEnabled = false;
        //model.QueueFree();
        model.AnimateDefeat();
        model = null;
    }

    public void Initiate(BattleEngine battleEngine)
    {
        this.battleEngine = battleEngine;
    }

    public MonsterModel GetModel()
    {
        if (model == null)
        {
            model = GD.Load<PackedScene>("res://Objects/Monster/" + this.GetType().Name + ".tscn").Instantiate<MonsterModel>();
            model.SetOwner(this);
        }
        return model;
    }

    public Node ExecuteTurn(BattleEngine battleEngine)
    {
        skills[targetSkill].Use(battleEngine, this, battleEngine.GetPartyMember(targetCharacter));
        string targetName = battleEngine.GetPartyMember(targetCharacter).name;
        battleEngine.Ui.ShowCenterMessage(name + " uses " + skills[targetSkill].name + " on " + targetName + "!");
        Node2D animation = skills[targetSkill].GetAnimation();
        battleEngine.Ui.SpawnEffectParty(animation, battleEngine.GetPartyMember(targetCharacter));
        ExecuteTurnAdditional();
        return animation;
    }

    // play animation or other unique animations a Monster inherited class would want when it performs its turn.
    protected virtual void ExecuteTurnAdditional() {}
    public void NotifyBattleEngine(float targetEfficiency)
    {
        battleEngine.ExecuteSkill(this, targetEfficiency);
    }

    public void Hit(MonsterTarget target)
    {
        if (targettingEnabled) TargetHit(target);
    

    }
    public void TargetMiss()
    {
        if (targettingEnabled) battleEngine.PlayerMissed(this);
    }

    protected abstract void TargetHit(MonsterTarget target);
    public abstract void LoadUpcomingTurn(BattleEngine battleEngine);
}