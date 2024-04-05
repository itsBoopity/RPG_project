using Godot;

[GlobalClass]
public abstract partial class SkillCustomWindow : Control
{
	[Signal]
	public delegate void CancelWindowEventHandler();

	[Signal]
	public delegate void ReturnDataEventHandler(BattleInteractionData data);

	public abstract void Open(BattleFieldData bF, BattleInteractionData bI);

	/// <summary>
	/// Closing the window after successful input, as opposed to cancellation, in which case <see cref="Cancel"/> is called.
	/// </summary>
	public void Close()
	{
		CleanUp();
		QueueFree();
	}

	/// <summary>
	/// Called when the window is cancelled. In other words, expected input entry didn't occur or wasn't finished.
	/// </summary>
	public void Cancel()
	{
		CancelCleanUp();
		CleanUp();
		QueueFree();
		EmitSignal(SignalName.CancelWindow);
	}

	/// <summary>
	/// Clean up that happens right before the window is removed, regardless of if after successful input, or cancelled.
	/// </summary>
	protected abstract void CleanUp();

	/// <summary>
	/// Clean up that happens when window was cancelled. Called right before window is closed and before <see cref="CleanUp"/>.
	/// </summary>
	protected abstract void CancelCleanUp();
}
