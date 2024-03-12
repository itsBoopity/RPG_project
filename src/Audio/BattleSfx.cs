using Godot;

/// <summary>
/// Contains the main Sfx elements of BattleEngine UI.
/// </summary>
public partial class BattleSfx : AudioPoolPlayer
{
	private AudioStreamPlayer strongClick;
	private AudioStreamPlayer rollClick;
	private AudioStreamPlayer errorSound;
	public void StrongClick()
	{
		PlayIndex(0);
	}
	/// <summary>
	/// Plays strong click sfx with increasing pitch depending on index.
	/// </summary>
	/// <param name="index"></param>
	public void StrongClickPitchIndex(int index)
	{
		PitchScale = 1.0f + index * 0.05f;
		PlayIndex(0);
	}
	public void RollClick()
	{
		PlayIndex(1);
	}
	/// <summary>
	/// Plays roll click sfx with increasing pitch depending on index.
	/// </summary>
	/// <param name="index"></param>
	public void RollClickPitchIndex(int index)
	{
		PitchScale = 1.0f + index * 0.05f;
		PlayIndex(1);
	}
	public void ErrorSound()
	{
		PlayIndex(2);
	}
}
