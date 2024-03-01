using Godot;

public partial class CharacterBar : Control
{
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

        GetNode<Label>("Hotkey").Text = ((InputEvent)InputMap.ActionGetEvents("battle_character" + Name)[0]).AsText();
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
        if (who != character.who) {
            who = character.who;
            name.Text = Utility.CharacterBBName(character.who);
        }

        hp.Text = character.hp.ToString();
        maxHp.Text = character.maxHp.ToString();
        stack.Text = character.stack.ToString();

        if (character.turnActive && !this.turnActive)
            fadePlayer.Play("RestoreTurn");
        else if (!character.turnActive && this.turnActive)
            fadePlayer.Play("UsedTurn");

        this.turnActive = character.turnActive;
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
