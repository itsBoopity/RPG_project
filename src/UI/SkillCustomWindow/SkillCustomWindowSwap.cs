using Godot;
using System.Linq;

public partial class SkillCustomWindowSwap : SkillCustomWindow
{
	private StretchArrow stretchArrow;
	private Node partyNode;
	private Node benchNode;

	private BattleFieldData bF;
	private BattleCharacter user;


	public override void _Ready()
    {
        partyNode = GetNode<Node>("Party");
		benchNode = GetNode<Node>("Bench");
    }

	public override void Open(BattleFieldData bF, BattleInteractionData bI)
	{
		this.bF = bF;
		this.user = (BattleCharacter)bI.user;
		LoadParty(bF.party, user);
		LoadBench(bF.bench);
		stretchArrow.Enable();
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
	}

	protected override void CleanUp()
	{
		stretchArrow.Disable();
	}

	/// <summary>
	/// Loads character data into Party UI, and simultaneously updates the UI to display the swap user.
	/// </summary>
	/// <param name="party"></param>
	/// <param name="toSwap"></param>
	public void LoadParty(CharacterRack party, BattleCharacter toSwap)
	{
		for (int i=0; i<3; i++)
		{
			if (i < party.Count)
			{
				partyNode.GetChild<CharacterBarSmall>(i).Update(party[i]);
				if (party[i].Who == toSwap.Who)
				{
					partyNode.GetChild<CharacterBarSmall>(i).SetAsSource();
					InstantiateStretchArrow(party[i]);
				}
				else
				{
					partyNode.GetChild<CharacterBarSmall>(i).SetAsTarget();
				}
				partyNode.GetChild<CharacterBarSmall>(i).Show();
			}
			else
			{
				partyNode.GetChild<CharacterBarSmall>(i).Hide();
			}
		}
		
	}

	public void LoadBench(CharacterRack bench)
	{
		for (int i=0; i<3; i++)
		{
			if (i < bench.Count)
			{
				benchNode.GetChild<CharacterBarSmall>(i).Update(bench[i]);
				benchNode.GetChild<CharacterBarSmall>(i).Show();
			}
			else
			{
				benchNode.GetChild<CharacterBarSmall>(i).Hide();
			}
		}
		
	}

	/// <summary>
	/// Instantiates stretch arrow tied to CharacterBarSmall.
	/// </summary>
	/// <param name="user">The character to tie the StretchArrow to.</param>
	public void InstantiateStretchArrow(BattleCharacter user)
	{
		foreach (CharacterBarSmall characterBar in partyNode.GetChildren().Cast<CharacterBarSmall>())
		{
			if (characterBar.Who == user.Who)
			{
				stretchArrow = GD.Load<PackedScene>("res://Objects/UI/StretchArrow.tscn").Instantiate<StretchArrow>();
				characterBar.AddChild(stretchArrow);
				stretchArrow.Position = characterBar.Size / 2;
				return;
			}
		}
	}

	public void CharacterSelected(string partyOrBench, int index)
	{
		if (partyOrBench == "Party")
		{
			EmitSignal(SignalName.ReturnData, new BattleInteractionData(user, bF.party[index]));
		}
		else
		{
			EmitSignal(SignalName.ReturnData, new BattleInteractionData(user, bF.bench[index]));
		}
	}
	
	protected override void CancelCleanUp() {}
}
