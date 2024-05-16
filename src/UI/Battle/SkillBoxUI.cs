using Godot;

public partial class SkillBoxUI : Control
{
    [Signal]
    public delegate void ShowSkillDetailEventHandler(int index);
    [Signal]
    public delegate void HideSkillDetailEventHandler();
    [Signal]
    public delegate void SelectSkillEventHandler(int index);

    /// <summary>
    /// The index of the skill that this skillbox represents. Used to signal to the engine which button was pressed, as well as to map the appropriate shortcut key.
    /// </summary>
    [Export]
    private int index = 0;
    /// <summary>
    /// Whether the button is part of the second row of keys. Used when mapping to hotkeys to determine which button to use.
    /// </summary>
    [Export]
    private bool secondRow = false;
    private TextureRect rootTexture;
    private TextureRect fill;
    private TextureRect icon;
    private Label stackCost;
    private Label cooldown;
    private CanvasItem snap;
    private Label cooldownCountdown;

    private Vector2 originalScale;
    private AudioStreamPlayer sfx;
    
    public override void _Ready()
    {
        rootTexture = GetNode<TextureRect>("RootTexture");
        fill = GetNode<TextureRect>("RootTexture/Fill");
        icon = GetNode<TextureRect>("RootTexture/Fill/Icon");
        stackCost = GetNode<Label>("RootTexture/Cost");
        cooldown = GetNode<Label>("RootTexture/Cooldown");
        snap = GetNode<CanvasItem>("RootTexture/SnapIcon");
        cooldownCountdown = GetNode<Label>("RootTexture/Countdown");
        sfx = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

        originalScale = rootTexture.Scale;

        if (!secondRow)
        {
            GetNode<Label>("RootTexture/Hotkey").Text = InputMap.ActionGetEvents("battle_skill" + index)[0].AsText().TrimSuffix(" (Physical)");
        }
        else
        {
            {
            GetNode<Label>("RootTexture/Hotkey").Text = InputMap.ActionGetEvents("battle_basicskill" + index)[0].AsText().TrimSuffix(" (Physical)");
        }
        }
    }

    public void OnHover()
    {
        sfx.Play();
        rootTexture.Scale = originalScale * 1.1f;
        EmitSignal(SignalName.ShowSkillDetail, index);
    }
    public void ExitHover()
    {
        rootTexture.Scale = originalScale;
        EmitSignal(SignalName.HideSkillDetail);
    }
    
    public void OnPress() {
        sfx.Play();
        EmitSignal(SignalName.SelectSkill, index);
    }

    public void Initiate(BattleSkill skill)
    {
        if (skill == null)
        {
            Empty();
            return;
        }
        else
        {
            fill.Show();
            stackCost.Show();
            cooldown.Show();
            cooldownCountdown.Hide();
        }
        
        icon.Texture = skill.Icon;

        fill.Texture = GD.Load<Texture2D>("res://Images/UI/Battle/Fill" + skill.Type.ToString() + ".png");
        
        if (skill.Type == SkillType.BASIC)
        {
            stackCost.Hide();
            cooldown.Hide();
            snap.Hide();
        }
        else
        {
            stackCost.Text = skill.Cost.ToString();
            cooldown.Text = skill.Cooldown.ToString();
            if (skill.Snap)
                snap.Show();
            else
                snap.Hide();
        }
    }

    public void Update(BattleCharacter owner, BattleSkill skill)
    {
        Initiate(skill);
        
        SkillUsableResult result = skill.IsUsable(owner);
        if (result == SkillUsableResult.IN_COOLDOWN)
        {
            fill.Modulate = new Color(0.4f, 0.4f, 0.4f, 1);
            cooldownCountdown.Show();
            cooldownCountdown.Text = skill.CurrentCooldown.ToString();
        }
        else if (result == SkillUsableResult.NOT_ENOUGH_STACKS || result == SkillUsableResult.CHARACTER_NOT_ACTIVE)
        {
            fill.Modulate = new Color(0.4f, 0.4f, 0.4f, 1);
        }
        else
        {
            fill.Modulate = Colors.White;
            cooldownCountdown.Hide();
        }
    }

    public void Empty()
    {
        fill.Hide();
        stackCost.Hide();
        cooldown.Hide();
        cooldownCountdown.Hide();
        snap.Hide();
    }
}
