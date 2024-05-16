using System.Linq;
using Godot;

public partial class SaveWindow : ColorRect
{
	public enum Mode
	{
		SAVE,
		LOAD
	}

	[Export]
	public Mode mode;

    public override void _Ready()
    {
		if (mode == Mode.SAVE)
		{
			GetNode<Label>("%Title").Text = "T_UI_SAVE_TITLE";
		}
        else
		{
			GetNode<Label>("%Title").Text = "T_UI_LOAD_TITLE";
		}
    }

    public override void _GuiInput(InputEvent @event)
    {
        if(@event.IsActionPressed("ui_cancel"))
		{
			Close();
			AcceptEvent();
		}
    }

    public void Open()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
		SetProcessUnhandledKeyInput(true);
		foreach (SaveSlotUI slot in GetNode("%SaveSlots").GetChildren().Cast<SaveSlotUI>())
		{
			SaveFileHeader? header = new SaveFileImporter().ImportHeader($"save{slot.SlotIndex}.dat");
			if (header.HasValue)
			{
				slot.LoadSaveHeader(header.Value);
			}
			else
			{
				slot.LoadEmpty();
			}
		}			
	}

	public void Close()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("RESET");
		GetNode<AudioPoolPlayer>("AudioStreamPlayer").PlayIndex(0);
		SetProcessUnhandledKeyInput(false);
	}

	public void SlotSelected(int index)
	{
		if (mode == Mode.SAVE)
		{
			SaveGame(index);
		}
		else
		{
			LoadGame(index);
		}
	}

	public void SaveGame(int index)
	{
		new SaveFileExporter().Export($"save{index}.dat");
		GlobalAudio.Instance.PlaySfx(GD.Load<AudioStream>("res://Audio/SFX/savesound.wav"));
		Close();
	}

	public void LoadGame(int index)
	{
		new SaveFileImporter().Import($"save{index}.dat");
	}
}
