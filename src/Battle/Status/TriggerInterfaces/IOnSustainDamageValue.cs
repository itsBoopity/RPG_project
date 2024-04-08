public interface IOnSustainDamageValue : IOnTriggerBase
{
	/// <summary>
	/// The order that the effect shold get activated in.
	/// Effects of the same trigger event activate starting from lowest Order to the highest.
	/// </summary>
	public int OnSustainDamageValueOrder { get; }

	/// <summary>
	/// Triggers when sustaining damage, allowing to edit the value
	/// </summary>
	public void ActivateSustainDamageValue(BattleFieldData bF, ref int damageValue);
}