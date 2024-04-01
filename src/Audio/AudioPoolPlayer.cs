using Godot;
using Godot.Collections;

/// <summary>
/// An extension of AudioStreamPlayer that allows you to define an array of sounds.
/// </summary>
public partial class AudioPoolPlayer : AudioStreamPlayer
{
	[Export]
	public Array<AudioStream> AudioSource { get; set; }

	/// <summary>
	/// Whether AudioPoolPlayer should play a random sound upon spawning.
	/// </summary>
	[Export]
	public bool autoRandom { get; set; }

    public override void _Ready()
    {
        if (autoRandom)
		{
			PlayRandom(0.2f);
		}
    }


    public void PlayIndex(int index)
	{
		Stream = AudioSource[index];
		Play();
	}


	public void PlayRandom(float deviatePitch = 0.0f)
	{
		PitchScale = 1.0f - deviatePitch/2 + GD.Randf() * deviatePitch;
		Stream = AudioSource[(int)(GD.Randi() % AudioSource.Count)];
		Play();
	}
}
