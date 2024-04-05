using Godot;
using Newtonsoft.Json;


[GlobalClass]
public partial class MonsterCard: DungeonCardData
{
    [Export]
    [JsonProperty]
    public bool IsBossBattle { get; set; }

    [Export]
    [JsonProperty]
    public BattleSetup Setup { get; set; }

    public MonsterCard() {}

    public override string GetName()
    {
        if (IsBossBattle)
        {
            return "T_DCARD_BOSSTITLE";
        }
        else
        {
            return "T_DCARD_MONSTER";
        }
    }

    public override string GetDescription()
    {
        if (IsBossBattle)
        {
            return "Defeat this foe to complete the dungeon!";
        }
        else
        {
            return "Enemies block the way!";
        }
    }

    public override Texture2D GetImage()
    {
        return GD.Load<Texture2D>("res://Images/UI/Dungeon/DungeonCard/monster_card.png");
    }

    public override void UseCard()
    {
        EmitSignal(SignalName.StartBattle, Setup);
    }

}
