using Godot;
using Newtonsoft.Json;

[GlobalClass]
[JsonConverter(typeof(DungeonSetupSerializer))]
public partial class DungeonSetup: Resource
{
	[Export]
	public string DungeonName { get; set; }

	[Export]
	public AudioStreamOggVorbis Music { get; set; }
	
	[Export]
	public int StartingHandSize { get; set; }
	[Export]
	public DungeonCard BossCard { get; set; }
	[Export]
	public Godot.Collections.Array<DungeonCardBundle> DeckDefine { get; set; }
	public DungeonSetup() {}
}
