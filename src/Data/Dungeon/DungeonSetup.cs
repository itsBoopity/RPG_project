using Godot;
using Newtonsoft.Json;

[GlobalClass]
[JsonConverter(typeof(DungeonSetupSerializer))]
public partial class DungeonSetup: Resource
{
	[Export]
	public int StartingHandSize { get; set; }
	[Export]
	public DungeonCardData BossCard { get; set; }
	[Export]
	public Godot.Collections.Array<DungeonCardBundle> DeckDefine { get; set; }
	[Export]
	public AudioStreamOggVorbis Music { get; set; }
	public DungeonSetup() {}
}
