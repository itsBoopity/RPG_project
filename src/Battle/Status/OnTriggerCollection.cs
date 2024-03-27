using System.Collections.Generic;

/// <summary>
/// Container class holding all of the possible IOnTrigger lines.
/// </summary>
public class OnTriggerCollection
{
	public LinkedList<IOnSustainDamageValue> OnSustainDamageValue {get; private set; }

	// public void AddOnSustainDamageValue(IOnSustainDamageValue toAdd)
	// {
	// 	SortedInsert<IOnSustainDamageValue>(OnSustainDamageValue, toAdd);
	// }



	// private void SortedInsert<T>(LinkedList<T> list, T toAdd) where T: BattleStatusBase 
	// {
	// 	for (LinkedListNode<T> node=list.First; node!=null; node=node.Next)
	// 	{
	// 		if (toAdd.ActivationOrder >= node.Value.ActivationOrder)
	// 		{
	// 			list.AddBefore(node, toAdd);
	// 		}
	// 	}
	// }
}