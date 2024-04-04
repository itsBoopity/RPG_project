using Godot;

public partial class DungeonEngine : Control
{
    [Signal]
    public delegate void HandSizeChangedEventHandler(int newSize);

    private DungeonUI ui;
    private DungeonSetup setup;
    private DungeonHand hand;
    
    private PackedScene cardSpawn = GD.Load<PackedScene>("res://Objects/UI/Dungeon/DungeonCard.tscn");
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
    private int handSize;
    private bool controlLock = false;
    public override void _Ready()
    {
        ui = GetNode<DungeonUI>("%UI");
        hand = GetNode<DungeonHand>("%Hand");
        HandSizeChanged += ui.UpdateHandSize;
        SceneManager.Instance.BattleExited += AfterBattle;

        if (Global.debugMode) LoadTest();
    }

    private void LoadTest()
    {
        GlobalAudio.Instance.PlayMusic("Music/StreamingStreamingEverFlowing.ogg");
        Initiate(GD.Load<DungeonSetup>("res://Resources/Dungeon/Setup/TestSetup.tres"));
    }

    public void Initiate(DungeonSetup setup)
    {
        this.setup = setup;
        handSize = setup.StartingHandSize;
        controlLock = false;
        hand.Clear();
        setup.DeckSizeChanged += ui.UpdateCardsLeft;
        ui.UpdateHandSize(HandSize);
        ui.UpdateCardsLeft(setup.CardsLeft());
        DrawToFull();
    }

    public void StartBattle(BattleSetup setup)
    {
        SceneManager.Instance.StartBattle(setup);
    }

    public void RequestUseCard(DungeonCard card)
    {
        if (controlLock == false)
        {
            card.Activate();
            controlLock = true;
        }
    }

    public void UseCard(DungeonCard card)
    {
        controlLock = false;
        ConnectCardDataSignals(card.Data);
        card.Data.UseCard();
        DisconnectCardDataSignals(card.Data);
        card.QueueFree();
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
            DungeonCardData data = setup.DrawFromDeck();
            if (data != null)
            {
                DungeonCard card = DungeonCard.InstantiateFromData(data);
                hand.Add(card);
                card.RequestActivation += RequestUseCard;
                card.CardSelected += UseCard;
            }
            else
            {
                break;
            }
        }
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

    private void ConnectCardDataSignals(DungeonCardData card)
    {
        card.StartBattle += StartBattle;
    }
    private void DisconnectCardDataSignals(DungeonCardData card)
    {
        card.StartBattle -= StartBattle;
    }
}
