using Godot;
using Newtonsoft.Json;

public class GameSettings
{
    public string language = TranslationServer.GetLocale();

    public bool timerEnabled = true;
    public bool noTimerForAnalysis = false;
    public bool showElementalModuloResult = false;

    public int monsterScrollSpeed = 1200;
    public int monsterScrollSensitivity = 100;
    public float monsterScrollSmoothness= 0.05f;


    //How quickly enemy turns play Slider from 0.5 - 3;
    public float enemyTurnSpeed = 1f;


    // 0.02f slow, 0.01f normal, 0.005f fast
    public float dialogueSpeed = 0.005f;

    /// <summary>
    /// Loads a configuration file if it exists, otherwise it creates one.
    /// </summary>
    /// <returns></returns>
    public static GameSettings CreateOnStartup()
    {
        if (System.IO.File.Exists(ProjectSettings.GlobalizePath("user://settings.cfg")))
        {
            return Load();
        }
        else
        {
            GameSettings settings = new();
            settings.Save();
            return settings;
        }
    }

    /// <summary>
    /// Loads and returns an instance of GameSettings.
    /// </summary>
    /// <returns>The GameSettings loaded from the settings file.</returns>
    /// <exception cref="System.IO.IOException">Couldn't open file.</exception>
    public static GameSettings Load()
    {
        using FileAccess file = FileAccess.Open($"user://settings.cfg", FileAccess.ModeFlags.Read);
        if (file == null)
        {
            throw new System.IO.IOException("GameSettings::Load() Couldn't open file.");
        }
        GameSettings settings = JsonConvert.DeserializeObject<GameSettings>(file.GetLine());
        TranslationServer.SetLocale(settings.language);
        return settings;
    }

    /// <summary>
    /// Saves this GameSetting as a JSON file at "user://settings.cfg"
    /// </summary>
    /// <exception cref="System.IO.IOException"></exception>
    public void Save()
    {
        using FileAccess file = FileAccess.Open($"user://settings.cfg", FileAccess.ModeFlags.Write);
        if (file == null)
        {
            throw new System.IO.IOException("GameSettings::Save() Couldn't access file to write to.");
        }
        file.StoreString(JsonConvert.SerializeObject(this));
    }
}