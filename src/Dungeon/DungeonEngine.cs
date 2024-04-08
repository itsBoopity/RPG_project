using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public partial class DungeonEngine : Control, IMainScene
{
    [Signal]
    public delegate void HandSizeChangedEventHandler(int newSize);

    public MainSceneEnum MainSceneType => MainSceneEnum.DUNGEONENGINE;
    public bool MainSceneSerializable => true;
    public string MainSceneDescription => setup.DungeonName;

    private DungeonUI ui;
    
    [JsonProperty]
    private DungeonSetup setup;
    [JsonProperty]
    private DungeonDeck deck;
    [JsonProperty]
    private List<DungeonCard> hand = new();

    private int handSize;

    private DungeonCard currentlyPlayedCard = null;
    
    private readonly PackedScene cardSpawn = GD.Load<PackedScene>("res://Objects/UI/Dungeon/DungeonCard.tscn");

    [JsonProperty]
    private int HandSize
    {
        get { return handSize; }

        set
        {
            handSize = value;
            EmitSignal(SignalName.HandSizeChanged, value);
        }
    }

    private const int MAXHANDSIZE = 5;
    public override void _Ready()
    {
        ui = GetNode<DungeonUI>("%UI");
        SceneManager.Instance.BattleExited += AfterBattle;
    }

    public void Initiate(DungeonSetup setup)
    {
        this.setup = setup;
        handSize = setup.StartingHandSize;
        deck = new DungeonDeck(setup.DeckDefine, setup.BossCard);
        deck.DeckSizeChanged += ui.UpdateCardsLeft;
        hand.Clear();
        DrawToFull();
        ui.UpdateHandSize(HandSize);
        ui.UpdateCardsLeft(deck.CardsLeft());
        GlobalAudio.Instance.PlayMusic(setup.Music);
    }

    public void Initiate(DungeonEngine dungeon)
    {
        setup = dungeon.setup;
        handSize = dungeon.handSize;
        deck = dungeon.deck;
        deck.DeckSizeChanged += ui.UpdateCardsLeft;
        hand = dungeon.hand;
        ui.UpdateHand(hand);
        ui.UpdateHandSize(HandSize);
        ui.UpdateCardsLeft(deck.CardsLeft());
        GlobalAudio.Instance.PlayMusic(setup.Music);
    }

    public void StartBattle(BattleSetup setup)
    {
        SceneManager.StartBattle(setup);
    }

    public void UseCard(int index)
    {
        DungeonCard card = hand[index];
        hand.RemoveAt(index);
        ConnectCardDataSignals(card);
        card.UseCard();
        DisconnectCardDataSignals(card);
    }

    public void AfterBattle()
    {
        DrawToFull();
    }

    private void DrawToFull()
    {
        int toDraw = handSize - hand.Count;

        for (int i=0; i < toDraw; i++)
        {
            DungeonCard card = deck.DrawFromDeck();
            if (card != null)
            {
                hand.Add(card);
            }
            else
            {
                break;
            }
        }
        ui.UpdateHand(hand);
    }

    public void ExpandHandSize(int byHowMuch)
    {
        handSize += byHowMuch;
        if (handSize > MAXHANDSIZE)
        {
            handSize = MAXHANDSIZE;
        }
        DrawToFull();
    }

    /// <summary>
    /// Connect card's signals to self, so that it can interact with DungeonEngine
    /// by sending signals.
    /// </summary>
    /// <param name="card"></param>
    private void ConnectCardDataSignals(DungeonCard card)
    {
        card.StartBattle += StartBattle;
    }
    private void DisconnectCardDataSignals(DungeonCard card)
    {
        card.StartBattle -= StartBattle;
    }
}
