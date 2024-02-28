using Godot;
using System;

public partial class InfoLabel : Node
{
    private TextureRect icon;
    private RichTextLabel label;
    private AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        icon = GetNode<TextureRect>("Icon");   
        label = GetNode<RichTextLabel>("Label");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void UpdateUI(ControlState state)
    {
        if (state == ControlState.PLAYER_DEFAULT)
        {
            label.Text = "Player Turn";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/player.png");
        }
        else if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            label.Text = "Enemy Targetting";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/targetting.png");
        }
        else if (state == ControlState.PLAYER_TARGETTING_ALLY)
        {
            label.Text = "Ally Targetting";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/ally.png");
        }
        else if (state == ControlState.ENEMY_TURN)
        {
            label.Text = "Enemy Turn";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/enemy.png");
        }
        animationPlayer.Play("Swap");
    }
}
