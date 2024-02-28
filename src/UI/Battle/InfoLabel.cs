using Godot;
using System;

public partial class InfoLabel : Node
{
    private TextureRect icon;
    private RichTextLabel label;
    private AnimationPlayer animationPlayer;

    private ControlState currentState = ControlState.FULLY_DISABLED; 
    public override void _Ready()
    {
        TranslationServer.SetLocale("en");
        icon = GetNode<TextureRect>("Icon");   
        label = GetNode<RichTextLabel>("Label");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void UpdateUI(ControlState state)
    {
        GD.Print(Tr("T_B_IL_PT"));
        if (state == ControlState.PLAYER_DEFAULT)
        {
            label.Text = "T_B_IL_PT";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/player.png");
            if (currentState == ControlState.ENEMY_TURN)
            {
                animationPlayer.Play("Slide");
            }
            else
            {
                animationPlayer.Play("Swap");
            }
        }
        else if (state == ControlState.PLAYER_TARGETTING_ENEMY)
        {
            label.Text = "T_B_IL_ET";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/targetting.png");
            animationPlayer.Play("Swap");
        }
        else if (state == ControlState.PLAYER_TARGETTING_ALLY)
        {
            label.Text = "T_B_IL_AT";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/ally.png");
            animationPlayer.Play("Swap");
        }
        else if (state == ControlState.ENEMY_TURN)
        {
            label.Text = "T_B_IL_ETU";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/enemy.png");
            animationPlayer.Play("Slide");
        }
        currentState = state;
    }
}
