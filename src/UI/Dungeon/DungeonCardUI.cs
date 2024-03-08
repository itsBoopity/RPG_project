using Godot;

public partial class DungeonCardUI : Control
{
	private Control cardSprite;
    private AnimationPlayer hoverPlayer;
    private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		// cardSprite = GetNode<Control>("");
        hoverPlayer = GetNode<AnimationPlayer>("HoverPlayer");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

    public void SetCard(DungeonCardData card)
    {
        GetNode<Label>("Title").Text = card.GetName();
        GetNode<Label>("Description").Text = card.GetDescription();
    }

	public void Hover()
	{
		hoverPlayer.Play("Hover");
	}

	public void Unhover()
    {
        hoverPlayer.Play("Unhover");
    }

    public void OnButtonUp()
    {
        this.Scale = Vector2.One;
    }

	public void OnButtonDown()
    {
        this.Scale = new Vector2(0.97f, 0.97f);
    }

    public void DrawCard()
    {
        animationPlayer.Play("Draw");
    }

    public AnimationPlayer Activate()
    {
        animationPlayer.Play("Activate");
        return animationPlayer;
    }
}
