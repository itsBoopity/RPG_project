using Godot;
using System.Collections.Generic;

public enum BattleInteractionType
{
	USER_ONLY,
	USER_SINGLETARGET,
	USER_MULTIPLETARGET,
}

/// <summary>
/// Structure that contains data about an interaction between two actors, user and target.
/// Can be built to contain additional info using Add methods.
/// </summary>
public partial class BattleInteractionData: Resource
{
	public readonly BattleActor user = null;
	public readonly BattleActor target = null;
	public readonly List<BattleActor> targets = null;

	public readonly BattleInteractionType type;
	public float FloatValue { get; private set; } = 1.0f;
	public int IntValue { get; private set; } = 0;

	/// <summary>
	/// Create a BattleInteraction where there is a user and a single target
	/// </summary>
	/// <param name="c_user">The initiator of the interaction.</param>
	/// <param name="c_target">The target of the interaction.</param>
	public BattleInteractionData(BattleActor c_user, BattleActor c_target)
	{
		type = BattleInteractionType.USER_SINGLETARGET;
		user = c_user;
		target = c_target;
	}

	/// <summary>
	/// Create a BattleInteraction where there is only a user.
	/// </summary>
	/// <param name="c_user">The initiator of the interaction.</param>
	public BattleInteractionData(BattleActor c_user)
	{
		type = BattleInteractionType.USER_ONLY;
		user = c_user;
	}

	/// <summary>
	/// Create a BattleInteraction where there is a user and multiple targets.
	/// </summary>
	/// <param name="c_user">The initiator of the interaction.</param>
	public BattleInteractionData(BattleActor c_user, List<BattleActor> c_targets)
	{
		type = BattleInteractionType.USER_MULTIPLETARGET;
		user = c_user;
	}

	/// <summary>
	/// Attach float value to BattleInteractionData.
	/// </summary>
	public BattleInteractionData AddFloatValue(float floatValue)
	{
		FloatValue = floatValue;
		return this;
	}

	public BattleInteractionData AddIntValue(int intValue)
	{
		IntValue = intValue;
		return this;
	}
}