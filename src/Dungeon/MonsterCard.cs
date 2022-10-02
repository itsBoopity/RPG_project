using Godot;
using System;

public class MonsterCard: DungeonCard
{
    private BattleSetup setup;
    public MonsterCard(BattleSetup setup, bool isBossBattle = false) {
        this.setup = setup;
        if (isBossBattle)
        {
            name = "Boss Battle";
            description = "- Defeat this foe to complete the dungeon!";
        }
        else
        {
            name = "Monster Battle";
            description = "Enemies block the way!\n-";
            foreach(int monsterID in setup.monsterID)
            {
                description += " " + MonsterFactory.GetName(monsterID);
            }
        }
    }

    public override void UseCard(DungeonEngine dungeonEngine)
    {
        dungeonEngine.StartBattle(setup);
    }
}
