using Godot;
using System.Collections.Generic;

public class DungeonEngine : Node2D
{
    private int handSize = 5;
    private List<DungeonCard> deck;
    private List<DungeonCard> hand;
    private DungeonCard bossCard;
    
    private PackedScene cardSpawn;
    public override void _Ready()
    {
        cardSpawn = GD.Load<PackedScene>("res://Objects/UI/Dungeon/DungeonCard.tscn");
        if (Global.debugMode) LoadTest();
    }

    private void LoadTest()
    {
        List<DungeonCard> deckLoad = new List<DungeonCard>();
        // Fill with cards

        BattleSetup battleSetup = new BattleSetup(); battleSetup.monsterID.Add(0);
        DungeonCard bossLoad = new BossCard(battleSetup);
        Initiate(deckLoad, bossLoad);
    }


    public void Initiate(List<DungeonCard> deck, DungeonCard bossCard, int handSize = 5)
    {
        this.deck = deck;
        this.bossCard = bossCard;
        this.handSize = handSize;
    }

    public void StartBattle(BattleSetup setup)
    {
        GetNode<Global>("/root/Global").StartBattle(setup);
    }

    public void UseCard(int index)
    {
        // Play and wait for card to finish animation then
        hand[index].UseCard();
        
        hand[index] = DrawFromDeck();   // if deck is empty hide UI????? But how to synchronize UI and LinkedList shift in positions?
        //Play Animation of draw
    }

    private DungeonCard DrawFromDeck()
    {
        DungeonCard output = deck[deck.Count];
        deck.RemoveAt(deck.Count - 1);
        return output;
    }

    public void ExpandHandSize(int byHowMuch)
    {
        handSize += byHowMuch;
        if (handSize > 5) handSize = 5;

        while (hand.Count < handSize)
        {
            hand.Add(DrawFromDeck());

            GD.Print("[Unfinished] Spaawn DungeonCard ExpandHandSize doesn't add CardUI");
        }
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

    
}
