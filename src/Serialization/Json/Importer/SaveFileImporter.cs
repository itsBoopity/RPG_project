using System.IO;
using Godot;
using Newtonsoft.Json;

public class SaveFileImporter
{
    /// <summary>
    /// Loads and returns only the header of fileName.
    /// </summary>
    /// <param name="fileName">File name relative to "user://save/"</param>
    /// <returns>The SaveFileHeader if successful, otherwise null.</returns>
    public SaveFileHeader? ImportHeader(string fileName)
    {
        try
        {
            using Godot.FileAccess file = Godot.FileAccess.OpenEncryptedWithPass($"user://save/{fileName}", Godot.FileAccess.ModeFlags.Read, (string)ProjectSettings.GetSetting("Custom/save_pass"));
            return JsonConvert.DeserializeObject<SaveFileHeader>(file.GetLine());
        }
        catch
        {
            return null;
        }
    }

    public bool Import(string fileName)
    {
        using (Godot.FileAccess file = Godot.FileAccess.OpenEncryptedWithPass($"user://save/{fileName}", Godot.FileAccess.ModeFlags.Read, (string)ProjectSettings.GetSetting("Custom/save_pass")))
        {
            SaveFileHeader header = JsonConvert.DeserializeObject<SaveFileHeader>(file.GetLine());
            GameData.Load(JsonConvert.DeserializeObject<GameData>(file.GetLine()));

            if (header.mainScene == MainSceneEnum.DUNGEONENGINE)
            {
                DungeonEngine dungeon = JsonConvert.DeserializeObject<DungeonEngine>(file.GetLine());
                SceneManager.ContinueDungeon(dungeon);
            }
        }
        return true;
    }
}