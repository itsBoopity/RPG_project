using Godot;

public partial class InfoLabel : Control
{
    private TextureRect icon;
    private RichTextLabel label;
    private AnimationPlayer animationPlayer;

    private ControlState currentState = ControlState.FULLY_DISABLED; 
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
            label.Text = "T_B_IL_PT";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/player.png");
            
            if (currentState == ControlState.ENEMY_TURN)
            {
                this.Modulate = Colors.Transparent;
                animationPlayer.Play("SlideLeft");
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
        else if (state == ControlState.PLAYER_SELECTING_ENEMY_CUSTOMWINDOW)
        {
            label.Text = "T_B_IL_ES";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/targetting.png");
            animationPlayer.Play("Swap");
        }
        else if (state == ControlState.ENEMY_TURN)
        {
            label.Text = "T_B_IL_ETU";
            icon.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/InfoLabel/enemy.png");
            this.Modulate = Colors.Transparent;
            animationPlayer.Play("Slide");
        }
        currentState = state;
    }
}
