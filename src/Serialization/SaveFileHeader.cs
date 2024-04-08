/// <summary>
/// A struct containing data included in the save file header.
/// </summary>
public struct SaveFileHeader
{
	/// <summary>
	/// The date when the save was made.
	/// </summary>
	public string date;

	/// <summary>
	/// Game time in seconds.
	/// </summary>
	public ulong gameTime;

	/// <summary>
	/// The main scene that the save was made during.
	/// </summary>
	public MainSceneEnum mainScene;

	/// <summary>
	/// The description of the current main scene.
	/// </summary>
	public string mainSceneDescription;
}