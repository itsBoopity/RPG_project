using Godot;
using Newtonsoft.Json;

/// <summary>
/// Save file format: Json on each component, separated into each line.
/// SaveFileHeader
/// GameData
/// MainSceneData
/// </summary>
public class SaveFileExporter
{

    /// <summary>
    /// Exports the save as an encrypted JSON into a file.
    /// </summary>
    /// <param name="fileName">The filename. File will be saved to "user://save/{filename}"</param>
    /// <returns>True if saved successfully, false if unable to, such as in a scene that shouldn't be saved in the middle of.</returns>
    public bool Export(string fileName)
    {
        // Create save directory if it doesn't exist yet.
        System.IO.Directory.CreateDirectory(ProjectSettings.GlobalizePath("user://save/"));

        if (SceneManager.Instance.CurrentScene is IMainScene scene && scene.MainSceneSerializable)
        {
            using (FileAccess file = FileAccess.OpenEncryptedWithPass($"user://save/{fileName}",FileAccess.ModeFlags.Write, (string)ProjectSettings.GetSetting("custom/save_pass"))) 
            {
                SaveFileHeader header = new() {
                    date = Time.GetDateStringFromSystem(),
                    gameTime = Global.Instance.GameTime,
                    mainScene = scene.MainSceneType,
                    mainSceneDescription = scene.MainSceneDescription
                };
                file.StoreLine(JsonConvert.SerializeObject(header));
                file.StoreLine(JsonConvert.SerializeObject(GameData.Instance));
                file.StoreLine(JsonConvert.SerializeObject(scene));
            }
        }
        else
        {
            return false;
        }

        return true;
    }
}