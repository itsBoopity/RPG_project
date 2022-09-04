using Godot;
using System;

public class CharacterBar : Sprite
{
    private RichTextLabel name;
    private Sprite icon;
    private PlayerIcon playerIcon = null;
    private Label hp;
    private Label maxHp;
    private Label stack;
    
    private AnimationPlayer animationPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        name = GetNode<RichTextLabel>("Name");
        icon = GetNode<Sprite>("Icon");
        playerIcon = GetNode<PlayerIcon>("PlayerIcon");
        playerIcon.Hide();

        hp = GetNode<Label>("HPCurrent");
        maxHp = GetNode<Label>("HPMax");
        stack = GetNode<Label>("StackCurrent");

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        GetNode<Label>("Hotkey").Text = ((InputEvent)InputMap.GetActionList("battle_character" + Name)[0]).AsText();
    }

    public void Select()
    {
        animationPlayer.Stop();
        animationPlayer.Play("Select");
    }

    public void Unselect()
    {
        animationPlayer.Stop();
        animationPlayer.Play("RESET");
    }

    public void Update(Character character)
    {
        name.BbcodeText = Utility.CharacterBBName(character.who);
        if (character.who == CharacterEnum.Player)
        {
            icon.Hide();
            playerIcon.Show();
        }
        else
        {
            icon.Show();
            playerIcon.Hide();
        }

        hp.Text = character.HP.ToString();
        maxHp.Text = character.maxHP.ToString();
        stack.Text = character.stack.ToString();
    }
}
