public partial class GameSettings
{
    public bool timerEnabled = true;
    public bool noTimerForAnalysis = false;
    public bool displayElementalModuloResult = false;
    public bool redoOnGameOver = false;

    public int monsterScrollSpeed = 1200;
    public int monsterScrollSensitivity = 100;
    public float monsterScrollSmoothness= 0.05f;


    //How quickly enemy turns play Slider from 0.5 - 3;
    public float enemyTurnSpeed = 1f;


    // 0.02f slow, 0.01f normal, 0.005f fast
    public float dialogueSpeed = 0.005f;
}