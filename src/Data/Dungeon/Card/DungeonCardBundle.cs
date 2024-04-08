using Godot;

[GlobalClass]
public partial class DungeonCardBundle: Resource
{
    [Export]
    public DungeonCard Card { get; set; }

    [Export]
    public int Count { get; set; }

    public DungeonCardBundle() {}
}
