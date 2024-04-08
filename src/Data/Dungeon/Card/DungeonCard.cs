using Godot;
using Newtonsoft.Json;

[GlobalClass]
[JsonConverter(typeof(DungeonCardDataSerializer))]
public abstract partial class DungeonCard: Resource
{
    [Signal]
    public delegate void StartBattleEventHandler(BattleSetup setup);

    public DungeonCard() {}

    public abstract string GetName();
    public abstract string GetDescription();
    public abstract Texture2D GetImage();

    public abstract void UseCard();
}
