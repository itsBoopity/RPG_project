using Godot;

public partial class DungeonCard : BaseButton
{
    [Signal]
    public delegate void CardSelectedEventHandler(DungeonCard who);

    [Signal]
    public delegate void RequestActivationEventHandler(DungeonCard who);

    private DungeonCardUI ui;
    private AudioPoolPlayer audio;
    private DungeonCardData data;

    public DungeonCardData Data
    {
        get
        {
            return data;
        }
        private set
        {
            data = value;
            GetNode<DungeonCardUI>("%UI").SetCard(data);
        }
    }

    public override void _ExitTree()
    {
        audio.Stop();
    }

    public override void _Ready()
    {
        audio = GetNode<AudioPoolPlayer>("Audio");
        ui = GetNode<DungeonCardUI>("%UI");
        ui.DrawCard();
        audio.PlayIndex(1);
    }

    public static DungeonCard InstantiateFromData(DungeonCardData data)
    {
        DungeonCard card = GD.Load<PackedScene>("res://Objects/UI/Dungeon/DungeonCard.tscn").Instantiate<DungeonCard>();
        card.Data = data;
        return card;
    }


    public void OnPressed()
    {
        EmitSignal(SignalName.RequestActivation, this);
    }

    public async void Activate()
    {
        audio.PlayIndex(0);
        await ToSignal(ui.Activate(), AnimationPlayer.SignalName.AnimationFinished);
        EmitSignal(SignalName.CardSelected, this);
    }
}
