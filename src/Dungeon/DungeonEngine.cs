using Godot;
using System.Collections.Generic;

public partial class DungeonEngine : Control
{
    private int handSize;
    private List<DungeonCard> deck;
    private List<DungeonCard> hand;
    private DungeonCard bossCard; bool bossCardDrawn = false;


    private Node handUI;
    private Label handSizeLabel;
    private Label cardsLeftLabel;
    
    private PackedScene cardSpawn;
    private bool controlLock = false;
    private int lastIndexUsed = -1;
    public override void _Ready()
    {
        handUI = GetNode("%Hand");

        handSizeLabel = GetNode<Label>("%HandSizeCount");
        cardsLeftLabel = GetNode<Label>("%CardsLeftCount");

        cardSpawn = GD.Load<PackedScene>("res://Objects/UI/Dungeon/DungeonCard.tscn");

        if (Global.debugMode) LoadTest();
    }

    public override void _Input(InputEvent @event)
    {
        // DungeonCard hotkeys
        // if (@event.IsActionPressed("dungeon_card" + index) && this.Visible && !this.Disabled)
        //     EmitSignal(SignalName.Pressed);
        
    }

    private void LoadTest()
    {
        GlobalAudio.Instance.PlayMusic("Music/StreamingStreamingEverFlowing.ogg");

        List<DungeonCard> deckLoad = new List<DungeonCard>();

        BattleSetup twoSlimes = new();
        twoSlimes
            .AddMonsterId(0)
            .AddMonsterId(0);

        BattleSetup oneSlime = new();
        oneSlime
            .AddMonsterId(0);
        
        for (int i=0; i<4; i++)
        {
            deckLoad.Add(new MonsterCard(oneSlime));
        }

        DungeonCard bossLoad = new MonsterCard(twoSlimes, true);
        Initiate(deckLoad, bossLoad, 4);
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
        Global.Instance.CallDeferred(Global.MethodName.StartBattle, setup);
    }

    public async void UseCard(int index)
    {
        if (controlLock) return;
        lastIndexUsed = index;
        controlLock = true;
        await ToSignal(handUI.GetChild<DungeonCardUI>(lastIndexUsed).ActivateAnimation(), "animation_finished");
        controlLock = false;
        hand[index].UseCard(this);
    }

    public void AfterBattle()
    {
        try
        {
            hand[lastIndexUsed] = DrawFromDeck();
            handUI.GetChild<DungeonCardUI>(lastIndexUsed).DrawCard(hand[lastIndexUsed]);
        }
        catch
        {
            hand.RemoveAt(lastIndexUsed); 
            UpdateUIHand();
        }
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

    // This method is specifically used to draw into the player's hand. As such the boss card behaves that way
    private DungeonCard DrawFromDeck()
    {
        int deckCount = deck.Count;
        if (deckCount == 0)
        {
            if (!bossCardDrawn)
            {
                cardsLeftLabel.Text = "0";
                bossCardDrawn = true;
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

    // Similar to DrawFromDeck, except it is used in effects, such as discard the top deck etc. Therefore it does nothing if bosscard is drawn
    private DungeonCard TopFromDeck()
    {
        int deckCount = deck.Count;
        if (deckCount == 0)
        {
            throw new System.Exception("Unhandled exception: TopFromDeck() called when deck empty.");
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

    private void UpdateUIHand()
    {
        int i = 0, handCount = hand.Count;

        foreach(DungeonCardUI card in handUI.GetChildren())
        {
            if (i < hand.Count)
            {
                card.SetCard(hand[i]);
                card.ResetAnimations();
            }
            else
                card.Hide();
            i++;
        }
    }
    
}
