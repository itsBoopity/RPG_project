using Godot;
using System;

public class CharacterBar : Sprite
{
    int HP = 25;
    private Label hpLabel;
    private AnimationPlayer animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        hpLabel = GetNode<Label>("HPCurrent");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_left"))
        {
            HP -= 1;
            UpdateValues();
        }

        if (@event.IsActionPressed("ui_accept"))
        {
            Select();
        }
            
    }

    public void Select()
    {
        animationPlayer.Stop();
        animationPlayer.Play("Select");
    }

    public void UpdateValues()
    {
        hpLabel.Text = HP.ToString();
    }
}
