using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;

public class SaveFileExporter
{
    /// <summary>
    /// Export game save data into a file.
    /// </summary>
    /// <param name="fileName">The filename. File will be saved to "user://save/{filename}"</param>
    /// <returns>True if saved successfully, false if unable to, such as in a scene that shouldn't be saved in the middle of.</returns>
    public bool Export(string fileName)
    {
        Directory.CreateDirectory(ProjectSettings.GlobalizePath("user://save/"));


        if (SceneManager.Instance.CurrentScene is IMainScene scene && scene.Serializable)
        {
            GD.Print(JsonConvert.SerializeObject(scene));
        }
        else
        {
            return false;
        }

        using (StreamWriter file = new(ProjectSettings.GlobalizePath($"user://save/{fileName}")))
        {
            file.WriteLine(JsonConvert.SerializeObject(GameData.Instance));
        }


        return true;
    }

    public class Nonme
    {
        public int i = 64;
    }
}