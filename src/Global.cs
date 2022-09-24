using Godot;

public class Global : Node
{
    public static GameData data = new GameData();
    public static GameSettings settings = new GameSettings();

    private BattleEngine battleEngine = null;
    private Node sceneHold = null;

    private AnimationPlayer blackFade;
    public static bool debugMode = true; //Show a menu with various tools and settings

    public Node currentScene { get; set; }

    public override void _ExitTree()
    {
        if (battleEngine != null) battleEngine.QueueFree();
        if (sceneHold != null) sceneHold.QueueFree();
    }

    public override void _Ready()
    {
        if (debugMode) data.newSave();

        GD.Randomize();
        Input.SetCustomMouseCursor(GD.Load("res://Images/UI/Battle/reticle.png"), Input.CursorShape.Cross, new Vector2(128,128));

        battleEngine = GD.Load<PackedScene>("res://Scenes/BattleEngine.tscn").Instance<BattleEngine>();

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

		currentScene.QueueFree();
        currentScene = GD.Load<PackedScene>(toLoad).Instance();

        GetTree().Root.AddChild(currentScene);
        GetTree().CurrentScene = currentScene;
		blackFade.Play("FadeOut");
        
        GetTree().Root.GuiDisableInput = false;
	}

    public void StartBattle(BattleSetup setup)
    {
        CallDeferred(nameof(DeferredStartBattle), setup);
    }

    public void DeferredStartBattle(BattleSetup setup)
    {
        GetTree().Root.AddChild(battleEngine);
        battleEngine.Initiate(setup);

        if (sceneHold != null) sceneHold.QueueFree();
        
        sceneHold = currentScene;
        GetTree().Root.RemoveChild(currentScene);
        currentScene = battleEngine;
    }

    public void EndBattle()
    {
        CallDeferred(nameof(DeferredEndBattle));
    }

    public void DeferredEndBattle()
    {
        GetTree().Root.AddChild(sceneHold);
        currentScene = sceneHold;
        sceneHold = null;

        GetTree().Root.RemoveChild(battleEngine);
    }



}
