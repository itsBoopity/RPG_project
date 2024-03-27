public abstract class BattleStatusBase
{
	protected abstract int InitialDuration { get; }
	public int TurnsLeft { get; protected set; }

	public BattleStatusBase()
	{
		TurnsLeft = InitialDuration;
	}

	public void AdvanceStatus(BattleFieldData bF)
	{
		TurnsLeft -= 1;
	}

	public abstract void ConnectTriggers(BattleStatusArray array);
}