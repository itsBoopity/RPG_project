using Godot;

/// <summary>
/// Adds a "StartTimer" signal to the Timer node.
/// </summary>
public partial class CustomTimer : Timer
{
	[Signal]
	public delegate void StartedEventHandler(double timeSec);

	public void CustomStart(double timeSec = -1.0)
	{
		EmitSignal(SignalName.Started, timeSec);
		Start(timeSec);
	}
}
