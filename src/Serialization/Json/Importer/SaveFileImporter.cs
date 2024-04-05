using System.IO;
using Godot;
using Newtonsoft.Json;

public class SaveFileImporter
{
    public bool Import(string fileName)
    {
        using (StreamReader file = new(ProjectSettings.GlobalizePath($"user://save/{fileName}")))
        {
            GameData.Load(JsonConvert.DeserializeObject<GameData>(file.ReadLine()));
        }
        return true;
    }
}