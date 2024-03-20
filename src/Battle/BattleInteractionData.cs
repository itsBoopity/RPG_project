/// <summary>
/// Structure that contains data about an interaction between two actors, user and target. And an optional appendageCoef.
/// </summary>
public struct BattleInteractionData
{
	public readonly BattleActor user;
	public readonly BattleActor target;
	public readonly float appendageCoef;

	public BattleInteractionData(BattleActor c_user = null, BattleActor c_target = null, float c_appendageCoef = 1.0f)
	{
		user = c_user;
		target = c_target;
		appendageCoef = c_appendageCoef;
	}
}