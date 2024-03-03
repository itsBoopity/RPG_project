using Godot;

/// <summary>
/// Adds a "Started" and Stopped signal to the Timer node.
/// Use CustomStart() and CustomStop() methods.
/// </summary>
public partial class CustomTimer : Timer
{
	[Signal]
	public delegate void StartedEventHandler(double timeSec);

	[Signal]
	public delegate void StoppedEventHandler();


	// The original methods are not overridable...
	public void CustomStart(double timeSec = -1.0)
	{
		EmitSignal(SignalName.Started, timeSec);
		Start(timeSec);
	}

	public void CustomStop()
	{
		EmitSignal(SignalName.Stopped);
		Stop();
	}
}
