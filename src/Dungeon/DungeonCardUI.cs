using Godot;
using System;

public class DungeonCardUI : TextureRect
{
    [Export] NodePath root;
    [Export] int index;

    
    private Label title;
    private Label desc;

    private DungeonEngine dungeonEngine;
    private Control button;
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        title = GetNode<Label>("Title");
        desc = GetNode<Label>("Description");
        GetNode<Label>("Hotkey").Text = ((InputEvent)InputMap.GetActionList("dungeon_card" + index)[0]).AsText();

        dungeonEngine = GetNode<DungeonEngine>(root);
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        button = GetNode<Control>("Button");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("dungeon_card" + index) && this.Visible)
            OnButtonPressed();
    }

    public void OnHover()
    {
        button.RectScale = new Vector2(1.05f, 1.05f);
    }

    public void OnHoverExit()
    {
        button.RectScale = Vector2.One;
    }


    public void OnButtonDown()
    {
        this.RectScale = new Vector2(0.97f, 0.97f);
    }

    public void OnButtonUp()
    {
        this.RectScale = Vector2.One;
    }


    public void OnButtonPressed()
    {
        animationPlayer.Play("Activate");
        dungeonEngine.UseCard(index, animationPlayer);
    }

    public void Draw()
    {
        animationPlayer.Play("Draw");
    }
    public void SetCard(DungeonCard card)
    {
        animationPlayer.Play("RESET");
        title.Text = card.name;
        desc.Text = card.GetDescription();

        GD.Print("[Unfinished] DungeonCardUI.SetCard doesn't have all UI implemented yet");
    }


}
