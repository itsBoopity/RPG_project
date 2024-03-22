using Godot;

public partial class CharacterBar : Control
{
    [Signal]
    public delegate void SelectCharacterEventHandler(int index);

    /// <summary>
    /// The index of which of the three bars this is.
    /// </summary>
    [Export]
    public int index = 0;

    private CharacterEnum who = 0;
    public CharacterEnum Who { get { return who;} }

    private RichTextLabel name;
    private TextureRect icon;
    private Label hp;
    private Label maxHp;
    private Label stack;

    private bool turnActive = false;

    private CanvasItem selector;
    private AnimationPlayer selectPlayer;
    private AnimationPlayer fadePlayer;
    private AnimationPlayer shakePlayer;

    public override void _Ready()
    {
        name = GetNode<RichTextLabel>("Name");
        icon = GetNode<TextureRect>("Icon");

        hp = GetNode<Label>("HPCurrent");
        maxHp = GetNode<Label>("HPMax");
        stack = GetNode<Label>("StackCurrent");

        selector = GetNode<CanvasItem>("Selector");
        selectPlayer = GetNode<AnimationPlayer>("SelectPlayer");
        fadePlayer = GetNode<AnimationPlayer>("FadePlayer");
        shakePlayer = GetNode<AnimationPlayer>("ShakePlayer");

        GetNode<Label>("Hotkey").Text = ((InputEvent)InputMap.ActionGetEvents($"battle_character{index}")[0]).AsText();
    }

    public void OnPress()
    {
        EmitSignal(SignalName.SelectCharacter, index);
    }

    public void Select()
    {
        selector.Show();
        selectPlayer.Stop();
        selectPlayer.Play("Select");
    }

    public void Deselect()
    {
        selector.Hide();
        selectPlayer.Stop();
        selectPlayer.Play("RESET");
    }

    public void Update(BattleCharacter character)
    {
        if (who != character.Who) {
            who = character.Who;
            name.Text = Utility.CharacterBBName(character.Who);
            icon.Texture = GD.Load<Texture2D>($"res://Images/CharacterIcon/{character.Who}.png");
        }
        hp.Text = character.Health.ToString();
        maxHp.Text = character.MaxHealth.ToString();
        stack.Text = character.Stack.ToString();

        if (character.TurnActive && !this.turnActive)
            fadePlayer.Play("RestoreTurn");
        else if (!character.TurnActive && this.turnActive)
            fadePlayer.Play("UsedTurn");

        this.turnActive = character.TurnActive;
    }

    public void ShakeStack()
    {
        shakePlayer.Stop();
        shakePlayer.Play("ShakeStack");
    }

    public void ShakeHP()
    {
        shakePlayer.Stop();
        shakePlayer.Play("ShakeHP");
    }
}
