using Godot;
using System;

public class MainEngine : Node
{
	public GameData gameData;

	public MainEngine() { GD.Randomize(); gameData = new GameData(); }

	AnimationPlayer blackFade;

	public override void _Ready()
	{
		blackFade = GetNode<AnimationPlayer>("BlackFade/Fade");
		GetNode<CanvasItem>("BlackFade").Hide();
	}

	public async void ChangeScene(string toLoad)
	{
		Node instance = GD.Load<PackedScene>(toLoad).Instance();
		blackFade.Play("FadeIn");
		await ToSignal(blackFade, "animation_finished");

		GetChild(0).QueueFree();
		AddChild(instance);
		MoveChild(instance, 0);

		blackFade.Play("FadeOut");
	}

	// Always spawn scene as FIRST child - MoveChild(instance, 0);
}
