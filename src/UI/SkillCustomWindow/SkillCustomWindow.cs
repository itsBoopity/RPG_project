using Godot;

public abstract partial class SkillCustomWindow : Control
{
	[Signal]
	public delegate void CancelWindowEventHandler();

	[Signal]
	public delegate void ReturnDataEventHandler(BattleInteractionData data);

	public abstract void Open(BattleFieldData bF, BattleCharacter user);

	public void Close()
	{
		CleanUp();
	}

	protected abstract void CleanUp();

	public void Cancel()
	{
		EmitSignal(SignalName.CancelWindow);
	}
}
