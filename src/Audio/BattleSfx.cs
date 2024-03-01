using Godot;

/// <summary>
/// Contains the main Sfx elements of BattleEngine UI.
/// </summary>
public partial class BattleSfx : Node
{
	private AudioStreamPlayer strongClick;
	private AudioStreamPlayer rollClick;
	private AudioStreamPlayer errorSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		strongClick = GetNode<AudioStreamPlayer>("StrongClick");
		rollClick = GetNode<AudioStreamPlayer>("RollClick");
		errorSound = GetNode<AudioStreamPlayer>("ErrorSound");
	}

	public void StrongClick()
	{
		strongClick.Play();
	}
	/// <summary>
	/// Plays strong click sfx with increasing pitch depending on index.
	/// </summary>
	/// <param name="index"></param>
	public void StrongClickPitchIndex(int index)
	{
		strongClick.PitchScale = 1.0f + index * 0.05f;
		strongClick.Play();
	}
	public void RollClick()
	{
		rollClick.Play();
	}
	/// <summary>
	/// Plays roll click sfx with increasing pitch depending on index.
	/// </summary>
	/// <param name="index"></param>
	public void RollClickPitchIndex(int index)
	{
		rollClick.PitchScale = 1.0f + index * 0.05f;
		rollClick.Play();
	}
	public void ErrorSound()
	{
		errorSound.Play();
	}
}
