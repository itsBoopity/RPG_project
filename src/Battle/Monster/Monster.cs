using Godot;

public abstract class Monster: BattleFigure
{ 
    public string name;
    protected string monsterID;
    protected MonsterModel model = null;
    
    //Position of party member and skill index that is going to be used, decided when player turn starts
    protected int targetCharacter;
    protected int targetSkill;

    public MonsterModel GetModel()
    {
        if (model == null)
            model = GD.Load<PackedScene>("res://Objects/Monster/" + monsterID).Instance<MonsterModel>();
        return model;
    }

    public abstract void LoadUpcomingTurn(ref BattleEngine battleEngine);

    public void ExecuteTurn(ref BattleEngine battleEngine)
    {
        skills[targetSkill].Execute(battleEngine.party[targetCharacter], this);
    }
}