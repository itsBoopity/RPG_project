using Godot;

public partial class GlobalAudio : Node
{
    private static GlobalAudio _instance;
    private AudioStreamPlayer music;
    private AudioStreamPlayer ambient;
    private AudioStreamPlayer sfx;
    private AnimationPlayer musicFade;
    private AnimationPlayer ambientFade;

    public static GlobalAudio Instance => _instance;
    public override void _EnterTree()
    {
        if(_instance != null) this.QueueFree();
        _instance = this;
    }

    public override void _Ready()
    {
        music = GetNode<AudioStreamPlayer>("Music");
        ambient = GetNode<AudioStreamPlayer>("Ambient");
        musicFade = GetNode<AnimationPlayer>("Music/Fade");
        ambientFade = GetNode<AnimationPlayer>("Ambient/Fade");
        sfx = GetNode<AudioStreamPlayer>("SFX");
    }

    /// <summary> Starts playing music. </summary>
    /// <param name="audio"> The audio to play. </param>
    /// <param name="fadeSpeed">
    ///     Coeficient to multiply fading speed by. 1.0 speed is 300ms. Set to 0 for no fade.
    /// </param>
    public async void PlayMusic(AudioStreamOggVorbis audio, float fadeSpeed = 0.0f, bool loop = true)
    {
        musicFade.SpeedScale = fadeSpeed == 0.0f ? 32 : fadeSpeed;

        musicFade.Stop();
        musicFade.Play("FadeOut");
        await ToSignal(musicFade, AnimationPlayer.SignalName.AnimationFinished);

        audio.Loop = loop;
        music.Stream = audio;
        musicFade.Play("FadeIn");
    }

    public void StopMusic(float fadeSpeed = 1.0f)
    {
        musicFade.Stop();
        musicFade.SpeedScale = fadeSpeed == 0 ? 32 : fadeSpeed;
        musicFade.Play("FadeOut");
    }

    public async void PlayAmbient(AudioStreamOggVorbis audio, float fadeSpeed = 1)
    {
        ambientFade.SpeedScale = fadeSpeed;

        ambientFade.Stop();
        ambientFade.Play("FadeOut");
        await ToSignal(ambientFade, AnimationPlayer.SignalName.AnimationFinished);

        audio.Loop = true;
        ambient.Stream = audio;
        ambientFade.Play("FadeIn");
    }

    public void StopAmbient(float fadeSpeed = 1)
    {
        ambientFade.Stop();
        ambientFade.SpeedScale = fadeSpeed == 0 ? 32 : fadeSpeed;
        ambientFade.Play("FadeOut");
    }

    public void PlaySfx(AudioStream audio)
    {
        sfx.Stream = audio;
        sfx.Play();
    }
}
