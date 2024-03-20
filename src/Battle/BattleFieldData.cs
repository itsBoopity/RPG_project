/// <summary>
/// Structure that contains variables of battle actors.
/// </summary>
public struct BattleFieldData
{
	public readonly CharacterRack party;
    public readonly CharacterRack bench;
    public readonly MonsterRack monsters;


	public BattleFieldData(	CharacterRack c_party, CharacterRack c_bench, MonsterRack c_monsters)
	{
		party = c_party;
		bench = c_bench;
		monsters = c_monsters;
	}
}