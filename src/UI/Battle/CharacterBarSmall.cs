using Godot;

public partial class CharacterBarSmall : Button
{
    public CharacterEnum Who { get { return who;} }
	private CharacterEnum who = 0;
	private RichTextLabel name;
    private TextureRect icon;
    private Label hp;
    private Label maxHp;
    private Label stack;
	private CanvasItem selector;

	private AnimationPlayer animationPlayer;
	private AnimationPlayer selectorAnimationPlayer;
	private AudioStreamPlayer sfx;

	private bool reactToHover = true;

    public override void _Ready()
    {
        name = GetNode<RichTextLabel>("TextureRect/Name");
        icon = GetNode<TextureRect>("TextureRect/Icon");

        hp = GetNode<Label>("TextureRect/HPCurrent");
        maxHp = GetNode<Label>("TextureRect/HPMax");
        stack = GetNode<Label>("TextureRect/StackCurrent");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		selector = GetNode<CanvasItem>("Selector");
		selectorAnimationPlayer = GetNode<AnimationPlayer>("Selector/AnimationPlayer");
		sfx = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }

	public void Update(BattleCharacter character)
    {
		animationPlayer.Play("RESET");
        if (who != character.Who) {
            who = character.Who;
            name.Text = Utility.CharacterBBName(character.Who);
            icon.Texture = GD.Load<Texture2D>($"res://Images/CharacterIcon/{character.Who}.png");
        }
        hp.Text = character.Health.ToString();
        maxHp.Text = character.MaxHealth.ToString();
        stack.Text = character.Stack.ToString();
    }

	/// <summary>
	/// Makes the CharacterBar uninteractable and displays a border.
	/// </summary>
	public void SetAsSource()
	{
		Disabled = true;
		reactToHover = false;
		selectorAnimationPlayer.Play("ActiveSmall");
	}

	/// <summary>
	/// Makes the CharacterBar interactable and display a blue border.
	/// </summary>
	public void SetAsTarget()
	{
		Disabled = false;
		reactToHover = true;
		selector.Modulate = Colors.White;
		selectorAnimationPlayer.Play("RESET");
	}


	public void OnHover()
	{
		if (reactToHover)
		{
			animationPlayer.Play("Hover");
			selectorAnimationPlayer.Play("Active");
			sfx.Play();
		}
	}
	public void OnUnhover()
	{
		if (reactToHover)
		{
			animationPlayer.Play("RESET");
			selectorAnimationPlayer.Play("RESET");
		}
	}
}
