using Godot;

public abstract class Monster: BattleFigure
{ 
    BattleEngine battleEngine = null;

    public bool targettingEnabled = false; // Whether clicks on monsters should be read;
    public string name;
    protected MonsterModel model = null;

    // The default damage modifier that gets used when non targetting offensive skills are used 
    protected float defaultDamage = 0.5f;
    
    //Position of party member and skill index that is going to be used, decided when player turn starts
    protected int targetCharacter = 0;
    protected int targetSkill = 0;

    ~Monster() { if (model != null) model.QueueFree(); }

    public void Initiate(BattleEngine battleEngine) { this.battleEngine = battleEngine; }

    public MonsterModel GetModel()
    {
        if (model == null)
        {
            model = GD.Load<PackedScene>("res://Objects/Monster/" + this.GetType().Name + ".tscn").Instance<MonsterModel>();
            model.SetOwner(this);
        }
        return model;
    }
    public void UpdateUIModel() { model.UpdateHP(); }
    public void ShowEstimate(int damage) { model.ShowEstimate(damage); }
    public void HideEstimate() { model.HideEstimate(); }
    public void PlayDamage(int damage) { model.PlayDamage(damage); }
    public void PlayDamage(string text) { model.PlayDamage(text); }

    public Node ExecuteTurn(BattleEngine battleEngine)
    {
        skills[targetSkill].Use(battleEngine, this, battleEngine.party[targetCharacter]);

        // play animation or other unique animations a Monster inherited class would want; 
        ExecuteTurnUniqe();

        string targetName = (battleEngine.party[targetCharacter].who == CharacterEnum.Player) ?
            Global.data.avaData.name : battleEngine.party[targetCharacter].who.ToString();

        battleEngine.centerNarration.ShowText(name + " uses " + skills[targetSkill].name + " on " + targetName + "!");
        Node2D animation = skills[targetSkill].GetAnimation();
        battleEngine.SpawnEffectParty(animation, targetCharacter);
        return animation;
    }
    protected virtual void ExecuteTurnUniqe() {}

    public void NotifyBattleEngine(float targetEfficiency) { battleEngine.ExecuteSkill(this, targetEfficiency); }

    public void Hit(MonsterTarget target) { if (targettingEnabled) TargetHit(target); }
    public void TargetMiss() { if (targettingEnabled) battleEngine.PlayerMissed(this); }
    protected abstract void TargetHit(MonsterTarget target);

    public abstract void LoadUpcomingTurn(BattleEngine battleEngine);
}