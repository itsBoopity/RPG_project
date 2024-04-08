using Godot;

public partial class SaveSlotUI : Button
{
	[Signal]
	public delegate void SelectSlotEventHandler(int index);

	[Export]
	public int SlotIndex { get; set; } = 0;

    public override void _Ready()
    {
        GetNode<Label>("SlotName").Text = string.Format(Tr("T_UI_SAVESLOT_TITLE"), SlotIndex + 1);
    }

    public void LoadSaveHeader(SaveFileHeader header)
	{
		GetNode<Label>("EmptyNotice").Text = header.mainSceneDescription;
		GetNode<Label>("Date").Text = header.date;
		GetNode<Label>("GameTime").Text = ConvertSecondsToHhMm(header.gameTime);
	}

	public void LoadEmpty()
	{
		GetNode<Label>("EmptyNotice").Text = "T_UI_SAVE_EMPTY";
	}

	public static string ConvertSecondsToHhMm(ulong seconds)
	{
		return string.Format("{0, 0:D2}:{1, 0:D2}", seconds / 3600, (seconds / 60) % 60);
	}

	public void OnHover()
	{
		GetNode<NinePatchRect>("NinePatchRect").Modulate = new Color(2f, 1.5f, 1f);
		GetNode<AudioPoolPlayer>("Audio").PlayIndex(0);
	}
	public void OnUnhover()
	{
		GetNode<NinePatchRect>("NinePatchRect").Modulate = Colors.White;
	}

	public void OnPress()
	{
		EmitSignal(SignalName.SelectSlot, SlotIndex);
		GetNode<AudioPoolPlayer>("Audio").PlayIndex(1);
	}
}
