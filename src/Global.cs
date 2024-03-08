using Godot;

public partial class Global : Node
{
    [Signal]
    public delegate void BattleFinishedEventHandler();

    private static Global _instance;
    private static GameSettings settings = new GameSettings();

    private BattleEngine battleEngine = GD.Load<PackedScene>("res://Scenes/BattleEngine.tscn").Instantiate<BattleEngine>();

    public static bool debugMode = OS.IsDebugBuild();
    private Node currentScene = null;
    private Node sceneHold = null;

    public static Global Instance => _instance;
    public static GameSettings Settings { get => settings; set => settings = value; }

    public override void _EnterTree()
    {
        if (_instance != null) this.QueueFree();
        _instance = this;
    }

    public override void _ExitTree()
    {
        battleEngine?.QueueFree();
        sceneHold?.QueueFree();
    }

    public override void _Ready()
    {
        GD.Randomize();
        Input.SetCustomMouseCursor(GD.Load("res://Images/UI/Battle/reticle.png"), Input.CursorShape.Cross, new Vector2(128,128));
        currentScene = GetTree().CurrentScene;
        AddChild(battleEngine);
        CallDeferred(MethodName.RemoveChild, battleEngine);
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionPressed("fullscreen"))
            if (GetWindow().Mode == Window.ModeEnum.Fullscreen)
                GetWindow().Mode = Window.ModeEnum.Windowed;
            else
                GetWindow().Mode = Window.ModeEnum.Fullscreen;
    }

    /// <summary>
    /// Custom version of SceneTree.ChangeSceneToFile() that includes animated transition.
    /// </summary>
    /// <param name="path">Path to the new scene.</param>
    public void ChangeSceneToFile(string path)
    {
		CallDeferred(MethodName.DeferredChangeScene, path);
	}

	private void DeferredChangeScene(string path)
    {
		currentScene.QueueFree();
        currentScene = GD.Load<PackedScene>(path).Instantiate();
        GetTree().Root.AddChild(currentScene);
	}

    public void StartBattle(BattleSetup setup)
    {
        CallDeferred(MethodName.DeferredStartBattle, setup);
    }

    private void DeferredStartBattle(BattleSetup setup)
    {
        GetTree().Root.RemoveChild(currentScene);
        GetTree().Root.AddChild(battleEngine);
        sceneHold?.QueueFree();
        sceneHold = currentScene;
        currentScene = battleEngine;
        battleEngine.Initiate(setup);
    }

    public void EndBattle()
    {
        CallDeferred(MethodName.DeferredEndBattle);
    }

    private void DeferredEndBattle()
    {
        GetTree().Root.RemoveChild(battleEngine);
        GetTree().Root.AddChild(sceneHold);
        currentScene = sceneHold;
        sceneHold = null;
        EmitSignal(SignalName.BattleFinished);
    }



}
