using Godot;

public partial class StretchArrow : CanvasGroup
{
	private Line2D line;
	private Sprite2D end;
	public override void _Ready()
	{
		line = GetNode<Line2D>("Line2D");
		end = GetNode<Sprite2D>("End");
		Disable();
	}

    /// <summary>
    /// Update the arrow to follow the mouse every frame.
    /// </summary>
    public override void _Process(double delta)
	{
		line.SetPointPosition(1,ToLocal(GetGlobalMousePosition()));
		end.Position = line.Points[1];
		end.Rotation = line.Points[0].AngleToPoint(line.Points[1]);
	}

    /// <summary>
    /// Disable and hide the arrow 
    /// </summary>
    public void Disable()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		SetProcess(false);
	}

	public void Enable()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;
		SetProcess(true);
	}
}
