public partial class MonsterCard: DungeonCard
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
        }
        foreach(MonsterId monsterId in setup.MonsterIds)
        {
            description += " " + MonsterFactory.GetName(monsterId);
        }
    }

    public override void UseCard(DungeonEngine dungeonEngine)
    {
        dungeonEngine.StartBattle(setup);
    }
}
