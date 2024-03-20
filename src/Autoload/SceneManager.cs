using Godot;

public partial class SceneManager : Node
{
	[Signal]
    public delegate void BattleExitedEventHandler();

	private static SceneManager _instance;
	private BattleEngine battleEngine = GD.Load<PackedScene>("res://Scenes/BattleEngine.tscn").Instantiate<BattleEngine>();
	private Node currentScene = null;
    private Node heldScene = null;
	private Tween transitionTween;
	public static SceneManager Instance => _instance;

	public override void _EnterTree()
    {
        if (_instance != null) this.QueueFree();
        _instance = this;
    }

	public override void _ExitTree()
    {
        battleEngine?.QueueFree();
        heldScene?.QueueFree();
    }

    public override void _Ready()
    {
        currentScene = GetTree().CurrentScene;
		AddChild(battleEngine);
        CallDeferred(MethodName.RemoveChild, battleEngine);
    }

	/// <summary>
    /// Custom version of SceneTree.ChangeSceneToFile() that includes animated transition.
    /// </summary>
    /// <param name="path">Path to the new scene.</param>
    public void ChangeSceneToFile(string path)
    {
		CallDeferred(MethodName.DeferredChangeScene, path);
	}

	public void StartBattle(BattleSetup setup)
    {
        CallDeferred(MethodName.DeferredStartBattle, setup);
    }

	public void EndBattle()
    {
        CallDeferred(MethodName.DeferredEndBattle);
    }

	private void DeferredChangeScene(string path)
    {
		currentScene.QueueFree();
        currentScene = GD.Load<PackedScene>(path).Instantiate();
        GetTree().Root.AddChild(currentScene);
	}

    private void DeferredStartBattle(BattleSetup setup)
    {
        GetTree().Root.AddChild(battleEngine);
        heldScene?.QueueFree();
        heldScene = currentScene;
        currentScene = battleEngine;
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
        currentScene = heldScene;
        heldScene = null;
        EmitSignal(SignalName.BattleExited);
    }
}
