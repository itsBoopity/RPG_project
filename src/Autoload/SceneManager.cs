using Godot;

public partial class SceneManager : Node
{
	[Signal]
    public delegate void BattleExitedEventHandler();

	private static SceneManager _instance;
	private BattleEngine battleEngine = GD.Load<PackedScene>("res://Scenes/BattleEngine.tscn").Instantiate<BattleEngine>();
    private Node heldScene = null;
	private Tween transitionTween;
	public static SceneManager Instance => _instance;
    public Node CurrentScene { get; private set; } = null;

	public override void _EnterTree()
    {
        if (_instance != null) this.QueueFree();
        else _instance = this;
    }

	public override void _ExitTree()
    {
        battleEngine?.QueueFree();
        heldScene?.QueueFree();
    }

    public override void _Ready()
    {
        CurrentScene = GetTree().CurrentScene;
    }

	/// <summary>
    /// Custom version of SceneTree.ChangeSceneToFile() that includes animated transition.
    /// </summary>
    /// <param name="path">Path to the new scene.</param>
    public static void ChangeSceneToFile(string path)
    {
		_instance.CallDeferred(MethodName.DeferredChangeScene, path);
	}

    /// <summary>
    /// Switches the scene to dungeon and initiates it using the setup.
    /// </summary>
    /// <param name="setup">The dungeon to initiate.</param>
    public static void EnterDungeon(DungeonSetup setup)
    {
        _instance.CallDeferred(MethodName.DeferredEnterDungeon, setup);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dungeon"></param>
    public static void ContinueDungeon(DungeonEngine dungeon)
    {
        _instance.CallDeferred(MethodName.DeferredContinueDungeon, dungeon);
    }


	public static void StartBattle(BattleSetup setup)
    {
        _instance.CallDeferred(MethodName.DeferredStartBattle, setup);
    }

	public static void EndBattle()
    {
        _instance.CallDeferred(MethodName.DeferredEndBattle);
    }

    // =====================================================
    // Deferred calls start here.
    // CallDeferred means the method will be performed on the next "idle" frame rather than immediately.
    // This is so that all nodes that are currently running can safely finish whatever they were doing.

	private void DeferredChangeScene(string path)
    {
		CurrentScene.QueueFree();
        CurrentScene = GD.Load<PackedScene>(path).Instantiate();
        GetTree().Root.AddChild(CurrentScene);
	}

    private void DeferredEnterDungeon(DungeonSetup setup)
    {
        CurrentScene.QueueFree();
        DungeonEngine dungeonEngine = GD.Load<PackedScene>("res://Scenes/DungeonEngine.tscn").Instantiate<DungeonEngine>();
        CurrentScene = dungeonEngine;
        GetTree().Root.AddChild(dungeonEngine);
        dungeonEngine.Initiate(setup);
    }

    private void DeferredContinueDungeon(DungeonEngine dungeon)
    {
        CurrentScene.QueueFree();
        DungeonEngine dungeonEngine = GD.Load<PackedScene>("res://Scenes/DungeonEngine.tscn").Instantiate<DungeonEngine>();
        CurrentScene = dungeonEngine;
        GetTree().Root.AddChild(dungeonEngine);
        dungeonEngine.Initiate(dungeon);
    }

    private void DeferredStartBattle(BattleSetup setup)
    {
        GetTree().Root.AddChild(battleEngine);
        heldScene?.QueueFree();
        heldScene = CurrentScene;
        CurrentScene = battleEngine;
        battleEngine.Initiate(setup);
		if (heldScene is CanvasItem canvasScene)
		{
			transitionTween = CreateTween();
			transitionTween.TweenProperty(heldScene, "modulate", Colors.Transparent, 0.2);
			transitionTween.TweenCallback(Callable.From(() => 
				{
					GetTree().Root.RemoveChild(heldScene);
					canvasScene.Modulate = Colors.White;
				}));
		}
		else
		{
			GetTree().Root.RemoveChild(heldScene);
		}
	}

	private void DeferredEndBattle()
    {
        GetTree().Root.RemoveChild(battleEngine);
        GetTree().Root.AddChild(heldScene);
        CurrentScene = heldScene;
        heldScene = null;
        EmitSignal(SignalName.BattleExited);
    }
}
