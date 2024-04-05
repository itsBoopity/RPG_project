/// <summary>
/// Interface inehrited by main scenes.
/// Defines its own type, and whether it is serializable into save data.
/// </summary>
public interface IMainScene
{
	public MainSceneEnum Type { get; }
	public bool Serializable { get; }
}