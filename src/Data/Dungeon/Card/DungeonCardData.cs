using Godot;

[GlobalClass]
public abstract partial class DungeonCardData: Resource
{
    [Signal]
    public delegate void StartBattleEventHandler(BattleSetup setup);

    public DungeonCardData() {}

    public abstract string GetName();
    public abstract string GetDescription();
    public abstract void UseCard();
}
