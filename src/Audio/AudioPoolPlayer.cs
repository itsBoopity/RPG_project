using Godot;
using Godot.Collections;

/// <summary>
/// An extension of AudioStreamPlayer that allows you to define an array of sounds.
/// </summary>
public partial class AudioPoolPlayer : AudioStreamPlayer
{
	[Export]
	public Array<AudioStream> AudioSource { get; set; }


	public void PlayIndex(int index)
	{
		Stream = AudioSource[index];
		Play();
	}
	public void PlayRandom()
	{
		Stream = AudioSource[(int)(GD.Randi() % AudioSource.Count)];
		Play();
	}
}
