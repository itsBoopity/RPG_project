using Godot;

public partial class DungeonCardUI : Button
{
    [Signal]
    public delegate void SelectEventHandler(DungeonCardUI who);

    [Signal]
    public delegate void StartedAnimationEventHandler(DungeonCardUI who);

    private TextureRect cardSprite;
    private AnimationPlayer hoverPlayer;
    private AnimationPlayer animationPlayer;

    public static DungeonCardUI InstantiateFromData(DungeonCard data)
    {
        DungeonCardUI card = GD.Load<PackedScene>("res://Objects/UI/Dungeon/DungeonCard.tscn").Instantiate<DungeonCardUI>();
        card.SetCard(data);
        return card;
    }

	public override void _Ready()
	{
        cardSprite = GetNode<TextureRect>("BaseTexture");
        hoverPlayer = GetNode<AnimationPlayer>("HoverPlayer");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play("Draw");
    }

    public override void _ExitTree()
    {
        GetNode<AudioPoolPlayer>("Audio").Stop();
    }

    public void SetCard(DungeonCard card)
    {
        GetNode<Label>("BaseTexture/Title").Text = card.GetName();
        GetNode<TextureRect>("BaseTexture/Illustration").Texture = card.GetImage();
        GetNode<Label>("BaseTexture/Description").Text = card.GetDescription();
    }

	public void OnHover()
	{
		hoverPlayer.Play("Hover");
	}

	public void OnUnhover()
    {
        hoverPlayer.Play("Unhover");
    }

    public void OnButtonUp()
    {
        cardSprite.Scale = Vector2.One;
    }

	public void OnButtonDown()
    {
        cardSprite.Scale = new Vector2(0.97f, 0.97f);
    }

    public async void Activate()
    {
        animationPlayer.Play("Activate");
        EmitSignal(SignalName.StartedAnimation, this);
        await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
        QueueFree();
        EmitSignal(SignalName.Select, this);
    }
}
