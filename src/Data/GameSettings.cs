using Godot;

public partial class GameSettings: Resource
{
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

    public void Load()
    {
        ConfigFile config = new();
        if (config.Load("user://settings.cfg") != Error.Ok)
        {
            return;
        }
        
        TranslationServer.SetLocale((string)config.GetValue("Main", "language"));
        timerEnabled = (bool)config.GetValue("Gameplay", "timerEnabled");
        noTimerForAnalysis = (bool)config.GetValue("Gameplay", "noTimerForAnalysis");
        showElementalModuloResult = (bool)config.GetValue("Gameplay", "showElementalModuloResult");

        monsterScrollSpeed = (int)config.GetValue("Control", "monsterScrollSpeed");
        monsterScrollSensitivity = (int)config.GetValue("Control", "monsterScrollSensitivity");
        monsterScrollSmoothness= (float)config.GetValue("Control", "monsterScrollSmoothness");
    }

    public void Save()
    {
        ConfigFile config = new();
        
        config.SetValue("Main", "language", TranslationServer.GetLocale());
        config.SetValue("Gameplay", "timerEnabled", timerEnabled);
        config.SetValue("Gameplay", "noTimerForAnalysis", noTimerForAnalysis);
        config.SetValue("Gameplay", "showElementalModuloResult", showElementalModuloResult);
        config.SetValue("Control", "monsterScrollSpeed", monsterScrollSpeed);
        config.SetValue("Control", "monsterScrollSensitivity", monsterScrollSensitivity);
        config.SetValue("Control", "monsterScrollSmoothness", monsterScrollSmoothness);
        
        config.Save("user://settings.cfg");
    }
}