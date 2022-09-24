using Godot;
using System;

public class BossCard: DungeonCard
{
    private BattleSetup setup;
    public BossCard(BattleSetup setup) {
        this.setup = setup;
        name = "Boss Battle";
        description = "Defeat this foe to complete the dungeon!";

    }

    public override void UseCard()
    {
        dungeonEngine.StartBattle(setup);
    }
}
