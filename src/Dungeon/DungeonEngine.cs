using Godot;
using System.Collections.Generic;

public class DungeonEngine : Node2D
{
    private int handSize = 5;
    private List<DungeonCard> deck;
    private List<DungeonCard> hand;
    private DungeonCard bossCard; bool bossCardDrawn = false;


    private Node handUI;
    private Label handSizeLabel;
    private Label cardsLeftLabel;
    
    private PackedScene cardSpawn;
    public override void _Ready()
    {
        handUI = GetNode("Hand");

        handSizeLabel = GetNode<Label>("TopTab/HandSize/Current");
        cardsLeftLabel = GetNode<Label>("TopTab/CardsLeft/Current");

        cardSpawn = GD.Load<PackedScene>("res://Objects/UI/Dungeon/DungeonCard.tscn");

        if (Global.debugMode) LoadTest();
    }

    private void LoadTest()
    {
        List<DungeonCard> deckLoad = new List<DungeonCard>();
        // Fill with cards

        BattleSetup twoSlimes = new BattleSetup(); twoSlimes.monsterID.Add(0); twoSlimes.monsterID.Add(0);

        BattleSetup oneSlime = new BattleSetup(); oneSlime.monsterID.Add(0);
        deckLoad.Add(new MonsterCard(oneSlime));
        deckLoad.Add(new MonsterCard(oneSlime));
        deckLoad.Add(new MonsterCard(oneSlime));
        deckLoad.Add(new MonsterCard(oneSlime));
        deckLoad.Add(new MonsterCard(oneSlime));

        DungeonCard bossLoad = new MonsterCard(twoSlimes, true);
        Initiate(deckLoad, bossLoad);
    }


    public void Initiate(List<DungeonCard> deck, DungeonCard bossCard, int handSize = 5)
    {
        this.deck = deck;
        this.bossCard = bossCard;
        this.handSize = handSize;
        hand = new List<DungeonCard>();

        bossCardDrawn = false;

        handSizeLabel.Text = handSize.ToString();
        cardsLeftLabel.Text = deck.Count.ToString();
        DrawToFull();
    }

    public void StartBattle(BattleSetup setup)
    {
        Global glob = GetNode<Global>("/root/Global");
        glob.CallDeferred(nameof(glob.StartBattle), setup);
    }

    public async void UseCard(int index, AnimationPlayer animationPlayer)
    {
        // Play and wait for card to finish animation then

        await ToSignal(animationPlayer, "animation_finished");

        hand[index].UseCard(this);
        
        try
        {
            hand[index] = DrawFromDeck();
            if (hand[index] == bossCard)
                bossCardDrawn = true;
        }
        catch { hand.RemoveAt(index); }

        UpdateUIHand();
        
        handUI.GetChild<DungeonCardUI>(index).Draw();
    }

    private void DrawToFull()
    {
        int toDraw = handSize - hand.Count;

        for (int i=0; i < toDraw; i++)
        {
            try { hand.Add(DrawFromDeck()); }
            catch { break; }
        }
        UpdateUIHand();
    }

    private DungeonCard DrawFromDeck()
    {
        int deckCount = deck.Count;
        if (deckCount == 0)
        {
            if (!bossCardDrawn)
            {
                cardsLeftLabel.Text = "0";
                return bossCard;
            }
            else
                throw new System.Exception("Unhandled exception: DrawFromDeck() called when deck empty.");
        }
        
        DungeonCard output = deck[deckCount - 1];
        deck.RemoveAt(deckCount - 1);

        cardsLeftLabel.Text = (deck.Count + (bossCardDrawn ? 0 : 1)).ToString();
        return output;
    }

    public void ExpandHandSize(int byHowMuch)
    {
        handSize += byHowMuch;
        if (handSize > 5) handSize = 5;

        DrawToFull();
    }

    public void ShuffleDeck()
    {
        int n = deck.Count; if (n < 1) throw new System.Exception("Deck is being shuffled while already empty.");
        for (int i = n; i > 1; i--)
        {
            int index = (int)(GD.Randi() % n);
            DungeonCard endReplaced = deck[n-1];
            deck[n-1] = deck[index];
            deck[index] = endReplaced;          
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_button"))
            UpdateUIHand();
    }

    private void UpdateUIHand()
    {
        int i = 0, handCount = hand.Count;

        foreach(DungeonCardUI card in handUI.GetChildren())
        {
            if (i < hand.Count)
            {
                card.SetCard(hand[i]);
                card.Show();
            }
            else
            {
                card.Hide();
            }

            i++;
        }
    }
    
}
