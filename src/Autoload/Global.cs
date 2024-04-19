using Godot;

public partial class Global : Node
{
    public readonly static bool debugMode = OS.IsDebugBuild();
    private static Global _instance;

    public static Global Instance => _instance;
    public static GameSettings Settings { get; set; }

    private ulong _gameTime = 0;
    private ulong _timeSinceLastSave = 0;

    /// <summary>
    /// The game time of the current save in seconds.
    /// </summary>
    public ulong GameTime
    {
        // Update _gameTime and _timeSinceLastSave, then return value.
        get
        {
            _gameTime += (Time.GetTicksMsec() - _timeSinceLastSave) / 1000;
            _timeSinceLastSave = Time.GetTicksMsec();
            return _gameTime;
        }
        // Sets the _gameTime and updates _timeSinceLastSave to begin tracking time.
        set
        {
            _gameTime = value;
            _timeSinceLastSave = Time.GetTicksMsec();
        }
    }

    public override void _EnterTree()
    {
        if (_instance != null) this.QueueFree();
        _instance = this;
    }

    public override void _Ready()
    {
        GD.Print(ProjectSettings.GlobalizePath("user://"));
        GD.Randomize();
        Input.SetCustomMouseCursor(GD.Load("res://Images/UI/Battle/reticle.png"), Input.CursorShape.Cross, new Vector2(128,128));
        Settings = GameSettings.CreateOnStartup();
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionPressed("fullscreen"))
        {
            if (GetWindow().Mode == Window.ModeEnum.Fullscreen)
                GetWindow().Mode = Window.ModeEnum.Windowed;
            else
                GetWindow().Mode = Window.ModeEnum.Fullscreen;
        }
    }

    public static void Save(string fileName)
    {
        new SaveFileExporter().Export(fileName);
    }
    public static void Load(string fileName)
    {
        new SaveFileImporter().Import(fileName);
    }
}
