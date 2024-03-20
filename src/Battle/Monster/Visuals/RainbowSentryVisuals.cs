using Godot;

public partial class RainbowSentryVisuals : MonsterVisuals
{
	private Node[] gemRoots = new Node[3];

    public override void _Ready()
    {
		base._Ready();
        gemRoots[0] = GetNode<Node>("Sprite/ArmorFront/GemOrbit/0");
		gemRoots[1] = GetNode<Node>("Sprite/ArmorFront/GemOrbit/1");
		gemRoots[2] = GetNode<Node>("Sprite/ArmorFront/GemOrbit/2");
    }

    public void SpawnGem(int index, SkillElement element)
	{
		gemRoots[index].GetNode<Sprite2D>("Gem").Texture = GD.Load<Texture2D>($"res://Images/Monster/RainbowSentry/Gems/{element.ToString().ToLower()}.png");
		gemRoots[index].GetNode<CpuParticles2D>("BreakParticles").Texture =  GD.Load<Texture2D>($"res://Images/Monster/RainbowSentry/GemsShard/{element.ToString().ToLower()}.png");
		gemRoots[index].GetNode<AnimationPlayer>("GemAnimation").Play("PopUp");
	}

	public void BreakGem(int index)
	{
		gemRoots[index].GetNode<AnimationPlayer>("GemAnimation").Play("Break");
	}
	
}
