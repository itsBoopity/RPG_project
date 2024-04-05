using Godot;

public partial class Global : Node
{
    private static Global _instance;
    public readonly static bool debugMode = OS.IsDebugBuild();

    public static Global Instance => _instance;
    public static GameSettings Settings { get; set; }  = new GameSettings();

    public override void _EnterTree()
    {
        if (_instance != null) this.QueueFree();
        _instance = this;
    }

    public override void _Ready()
    {
        GD.Randomize();
        Input.SetCustomMouseCursor(GD.Load("res://Images/UI/Battle/reticle.png"), Input.CursorShape.Cross, new Vector2(128,128));
        Settings.Load();
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
        else if (@event.IsActionPressed("debug_f1"))
        {
            Save();
        }
        else if (@event.IsActionPressed("debug_f2"))
        {
            Load();
        }
    }

    public static void Save(string fileName = "save0.dat")
    {
        new SaveFileExporter().Export(fileName);
    }
    public static void Load(string fileName = "save0.dat")
    {
        new SaveFileImporter().Import(fileName);
    }

}
