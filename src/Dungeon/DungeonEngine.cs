using Godot;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public partial class DungeonEngine : Control, IMainScene
{
    [Signal]
    public delegate void HandSizeChangedEventHandler(int newSize);

    public MainSceneEnum Type => MainSceneEnum.DUNGEONENGINE;
    public bool Serializable => true;

    private DungeonUI ui;
    
    [JsonProperty]
    private DungeonSetup setup;

    [JsonProperty]
    private DungeonDeck deck;
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
    }

    public void Initiate(DungeonSetup setup)
    {
        GlobalAudio.Instance.PlayMusic(setup.Music);
        this.setup = setup;
        handSize = setup.StartingHandSize;
        deck = new DungeonDeck(setup.DeckDefine, setup.BossCard);
        deck.DeckSizeChanged += ui.UpdateCardsLeft;
        hand.Clear();
        DrawToFull();
        ui.UpdateHandSize(HandSize);
        ui.UpdateCardsLeft(deck.CardsLeft());
        controlLock = false;
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
            DungeonCardData data = deck.DrawFromDeck();
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
