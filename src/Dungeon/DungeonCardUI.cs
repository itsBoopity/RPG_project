using Godot;
using System;

public class DungeonCardUI : TextureRect
{
    [Export] NodePath root;
    [Export] int index;
    private DungeonEngine dungeonEngine;
    private Control button;
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        dungeonEngine = GetNode<DungeonEngine>(root);
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        button = GetNode<Control>("Button");

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
        //After disappearing notify dungeonEngine, which activates its effects, and then places in a new card.
        
    }

    public void SetCard(DungeonCard card)
    {

    }


}
