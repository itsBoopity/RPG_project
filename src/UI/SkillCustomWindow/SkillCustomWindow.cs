using Godot;

public abstract partial class SkillCustomWindow : Control
{
	[Signal]
	public delegate void ReturnIntEventHandler(int value);
	[Signal]
	public delegate void ReturnBattleCharacterEventHandler(BattleCharacter character);

	public abstract void Open(BattleFieldData bF, BattleCharacter user);

	public void Close()
	{
		CleanUp();
	}

	protected abstract void CleanUp();
}
