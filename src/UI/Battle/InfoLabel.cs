using Godot;
using System;

public class InfoLabel : RichTextLabel
{
    private Sprite icon;
    public override void _Ready()
    {
        icon = GetNode<Sprite>("Icon");   
    }

    public void UpdateUI(ControlState state)
    {
        if (state == ControlState.PLAYER_DEFAULT)
        {
            this.BbcodeText = "Player Turn";
            icon.Texture = GD.Load<Texture>("res://Images/UI/Battle/InfoLabel/player.png");
        }
        else if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            this.BbcodeText = "Enemy Targetting";
            icon.Texture = GD.Load<Texture>("res://Images/UI/Battle/InfoLabel/targetting.png");
        }
        else if (state == ControlState.PLAYER_TARGETTING_ALLY)
        {
            this.BbcodeText = "Ally Targetting";
            icon.Texture = GD.Load<Texture>("res://Images/UI/Battle/InfoLabel/ally.png");
        }
        else if (state == ControlState.ENEMY_TURN)
        {
            this.BbcodeText = "Enemy Turn";
            icon.Texture = GD.Load<Texture>("res://Images/UI/Battle/InfoLabel/enemy.png");
        }
    }
}
