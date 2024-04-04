using Godot;

public partial class SettingsWindow : Control
{
	private AudioPoolPlayer sfx;

    public override void _Ready()
    {
        sfx = GetNode<AudioPoolPlayer>("AudioStreamPlayer");
    }

    /// <summary>
    /// Open and update buttons to match settings data.
    /// </summary>
    public void Open()
	{
		sfx.PlayIndex(0);
		GetNode<CheckBox>("%DisableTimer").ButtonPressed = !Global.Settings.timerEnabled;
		GetNode<CheckBox>("%ShowMath").ButtonPressed = Global.Settings.showElementalModuloResult;
		UpdateLanguageOptionButton();
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Open");
	}
	public void Close()
	{
        Global.Settings.Save();
		Hide();
	}

	public void UpdateLanguageOptionButton()
	{
		string language = TranslationServer.GetLocale();
		int languageIndex = 0;

		if (language.StartsWith("en")) languageIndex = 0;
		else if (language.StartsWith("cs")) languageIndex = 1;

		GetNode<OptionButton>("%LanguageOptionButton").Selected = languageIndex;
	}

	public void SetTimerEnabled(bool buttonPressed)
	{
		sfx.PlayIndex(1);
		Global.Settings.timerEnabled = !buttonPressed;
	}

	public void SetShowElementalModuloResult(bool buttonPressed)
	{
		sfx.PlayIndex(1);
		Global.Settings.showElementalModuloResult = buttonPressed;
	}

	public void SetLanguage(int index)
	{
		sfx.PlayIndex(1);
		if (index == 0) TranslationServer.SetLocale("en");
		else if (index == 1) TranslationServer.SetLocale("cs");
	}
}
