using Godot;

public class Global : Node
{
    public static GameData data = new GameData();
    public static GameSettings settings = new GameSettings();

    private AnimationPlayer blackFade;
    public static bool debugMode = true; //Show a menu with various tools and settings

    public Node currentScene { get; set; }

    public override void _Ready()
    {
        GD.Randomize();

        Viewport root = GetTree().Root;
        currentScene = root.GetChild(root.GetChildCount() - 1);

        blackFade = GetNode<AnimationPlayer>("/root/BlackFade/AnimationPlayer");
    }

    public void ChangeScene(string toLoad)
	{
		// Only load next scene once all code is finished and executed.
        GetTree().Root.GuiDisableInput = true;
		CallDeferred(nameof(DeferredChangeScene), toLoad);
	}

	public async void DeferredChangeScene(string toLoad)
	{
        blackFade.Stop();
		blackFade.Play("FadeIn");
		await ToSignal(blackFade, "animation_finished");

		currentScene.Free();
        currentScene = GD.Load<PackedScene>(toLoad).Instance();

        GetTree().Root.AddChild(currentScene);
        GetTree().CurrentScene = currentScene;
		blackFade.Play("FadeOut");
        
        GetTree().Root.GuiDisableInput = false;
	}

}
