using Godot;
using System;

public partial class InfoLabel : RichTextLabel
{
    private Sprite2D icon;
    public override void _Ready()
    {
        icon = GetNode<Sprite2D>("Icon");   
    }

    public void UpdateUI(ControlState state)
    {
        if (state == ControlState.PLAYER_DEFAULT)
        {
            this.Text = "Player Turn";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/player.png");
        }
        else if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            this.Text = "Enemy Targetting";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/targetting.png");
        }
        else if (state == ControlState.PLAYER_TARGETTING_ALLY)
        {
            this.Text = "Ally Targetting";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/ally.png");
        }
        else if (state == ControlState.ENEMY_TURN)
        {
            this.Text = "Enemy Turn";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/enemy.png");
        }
    }
}
