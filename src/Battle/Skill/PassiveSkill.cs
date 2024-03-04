public abstract class PassiveSkill
{
    public string name;


    // Maybe do some kind of Subscriber system, where each passive adds itself to a list in battle engine
    // instead of cycling through all passives and empty method calls for every action;
    public void OnPlayerTurn(BattleEngine battleEngine, IBattleActor User) {}
    
}
