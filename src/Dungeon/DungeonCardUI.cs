using Godot;

public partial class DungeonCardUI : Control
{
    [Export]
    private DungeonEngine dungeonEngine;
    [Export(PropertyHint.Range, "0,4")]
    int index;

    
    private Control cardSprite;
    private Label title;
    private Label desc;
    private AnimationPlayer hoverPlayer;
    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer audio;
    private static AudioStreamWav[] sfxSrc = {
        GD.Load<AudioStreamWav>("res://Audio/SFX/cardUse.wav"),
        GD.Load<AudioStreamWav>("res://Audio/SFX/cardDraw.wav"),
        GD.Load<AudioStreamWav>("res://Audio/SFX/cardHover.wav")
    };

    public override void _ExitTree()
    {
        audio.Stop();
    }

    public override void _Ready()
    {
        cardSprite = GetNode<Control>("CardSprite");
        title = GetNode<Label>("CardSprite/Title");
        desc = GetNode<Label>("CardSprite/Description");
        GetNode<Label>("CardSprite/Hotkey").Text = ((InputEvent)InputMap.ActionGetEvents("dungeon_card" + index)[0]).AsText();

        hoverPlayer = GetNode<AnimationPlayer>("HoverPlayer");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        audio = GetNode<AudioStreamPlayer>("Audio");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("dungeon_card" + index) && this.Visible)
            OnButtonPressed();
    }

    public void OnHover()
    {
        audio.Stream = sfxSrc[2];
        audio.Play();
        hoverPlayer.Play("Hover");
    }

    public void OnHoverExit()
    {
        hoverPlayer.Play("Unhover");
    }


    public void OnButtonDown()
    {
        this.Scale = new Vector2(0.97f, 0.97f);
    }

    public void OnButtonUp()
    {
        this.Scale = Vector2.One;
    }

    public void OnButtonPressed()
    {
        dungeonEngine.UseCard(index);
    }

    public AnimationPlayer ActivateAnimation()
    {
        audio.Stream = sfxSrc[0];
        audio.Play();
        animationPlayer.Play("Activate");
        return animationPlayer;
    }

    public void DrawCard(DungeonCard card)
    {
        SetCard(card);
        animationPlayer.Play("Draw");
        audio.Stream = sfxSrc[1];
        audio.Play();
    }
    public void SetCard(DungeonCard card)
    {
        title.Text = card.name;
        desc.Text = card.GetDescription();
    }

    public void ResetAnimations()
    {
        animationPlayer.Play("RESET");
    }
}
