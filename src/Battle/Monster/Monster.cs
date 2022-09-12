using Godot;

public abstract class Monster: BattleFigure
{ 
    public string name;
    protected MonsterModel model = null;

    // The default damage modifier that gets used when 
    protected float defaultDamage = 0.5f;
    
    //Position of party member and skill index that is going to be used, decided when player turn starts
    protected int targetCharacter = 0;
    protected int targetSkill = 0;

    ~Monster() { model.Free(); }
    public MonsterModel GetModel()
    {
        if (model == null)
            model = GD.Load<PackedScene>("res://Objects/Monster/" + this.GetType().Name + ".tscn").Instance<MonsterModel>();
        model.Connect("Hit", this, "TargetHit");
        model.Connect("Miss", this, "TargetMiss");
        return model;
    }
    public void ExecuteTurn(BattleEngine battleEngine)
    {
        skills[targetSkill].Execute(this, battleEngine.party[targetCharacter]);
    }

    public void TargetMiss() { GD.Print("Monster.cs got missed, but it's not implemented yet."); }
    public abstract void LoadUpcomingTurn(BattleEngine battleEngine);
    public abstract void TargetHit(MonsterTarget target);
}