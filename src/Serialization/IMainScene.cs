/// <summary>
/// Interface inehrited by main scenes.
/// Defines the type of scene it is, and whether it is serializable into save data.
/// </summary>
public interface IMainScene
{
	/// <summary>
	/// The type of scene the node is.
	/// </summary>
	public MainSceneEnum MainSceneType { get; }

	/// <summary>
	/// Whether the node is able to be serialized.
	/// </summary>
	public bool MainSceneSerializable { get; }

	/// <summary>
	/// The Description of the scene to display in saves.
	/// </summary>
	public string MainSceneDescription { get; }
}