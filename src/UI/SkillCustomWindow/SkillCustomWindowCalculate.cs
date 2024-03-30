using Godot;
using System;

public partial class SkillCustomWindowCalculate : SkillCustomWindow
{
    private BattleCharacter user;
    private int value = 0;
    public override void Open(BattleFieldData bF, BattleCharacter c_user)
    {
        user = c_user;
        GetNode<Label>("LeftArrow/Hotkey").Text = InputMap.ActionGetEvents("ui_left")[0].AsText().TrimSuffix(" (Physical)");
        GetNode<Label>("RightArrow/Hotkey").Text = InputMap.ActionGetEvents("ui_right")[0].AsText().TrimSuffix(" (Physical)");
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event.IsActionReleased("ui_left"))
        {
            AdjustLeft();
        }
        else if (@event.IsActionReleased("ui_right"))
        {
            AdjustRight();
        }
        else if (@event.IsActionReleased("ui_accept"))
        {
            Confirm();
        }
    }

    private void AdjustLeft()
    {
        GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        if(value > -1)
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
            value -= 1;
            GetNode<Label>("Number").Text = value.ToString();
        }        
    }

    private void AdjustRight()
    {
        if(value < 1)
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
            value += 1;
            GetNode<Label>("Number").Text = value.ToString();
        }
    }

    private void Confirm()
    {
        EmitSignal(SignalName.ReturnData, new BattleInteractionData(user).AddIntValue(value));
    }

    protected override void CleanUp() {}

}
