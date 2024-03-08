using Godot;
using System;

[GlobalClass]
public partial class MonsterCard: DungeonCardData
{
    [Export]
    public bool IsBossBattle { get; set; }

    [Export]
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

    public override void UseCard()
    {
        EmitSignal(SignalName.StartBattle, Setup);
    }

}
