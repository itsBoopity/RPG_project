using System.Collections.Generic;

/// <summary>
/// Array that holds and manages Statuses added to it and any interactions required from them
/// </summary>
public partial class BattleStatusArray
{
	// If you do multiple Queues... how do you automatically add and remove them?
	// Obviously a BattleStatus knows what it is, but it can't call Add/Remove on all its Interfaces that it inherits
	
	// ==========================================================
	// If you do single queue, the queue will have to be scanned for every single call and an array
	// will have to be created of statuses subscribed to it every single time.
	// This sounds like the better option...
	private LinkedList<BattleStatusBase> statusList;

	private OnTriggerCollection triggerCollection = new();

	public void AddStatus(BattleStatusBase toAdd)
	{
		statusList.AddLast(toAdd);
	}

	/// <summary>
	/// Advances status turn count and erases ones that have run out.
	/// </summary>
	/// <param name="bF">BattleField data so that statuses can activate when they are IOnAdvanceStatus.</param>
	public void AdvanceStatuses(BattleFieldData bF)
	{
		for (LinkedListNode<BattleStatusBase> node=statusList.First; node!=null; node=node.Next)
		{
			node.Value.AdvanceStatus(bF);
			if (node.Value.TurnsLeft == 0)
			{
				LinkedListNode<BattleStatusBase> next = node.Next;
				RemoveStatus(node);
				node = next;
			}
		}
	}

	public void RemoveStatus(BattleStatusBase status)
	{
		statusList.Remove(status);
	}

	private void RemoveStatus(LinkedListNode<BattleStatusBase> status)
	{
		statusList.Remove(status);
	}
}