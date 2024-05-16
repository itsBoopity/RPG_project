using Godot;

/// <summary>
/// An extension of an AnimationPlayer that starts the Autoplayed animation at a random spot.
/// </summary>
public partial class AnimationPlayerRandomStart : AnimationPlayer
{
	public override void _Ready()
	{
		CallDeferred(MethodName.RandomizeCurrentAnimationPosition);
	}

	public void RandomizeCurrentAnimationPosition()
	{
		Seek(CurrentAnimationLength * GD.Randf(), true);
	}

}
