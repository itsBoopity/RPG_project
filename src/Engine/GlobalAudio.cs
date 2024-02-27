using Godot;
using System;

public partial class GlobalAudio : Node
{
    private static GlobalAudio _instance;
    private AudioStreamPlayer music;
    private AudioStreamPlayer ambient;
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
    }

    /// <summary>
    ///     Plays music.
    /// </summary>
    /// <param name="path">
    ///     Path to the audio file. Absolute if it begins with <c>"res://"</c>, otherwise relative to <c>"res://Audio/"</c>.
    /// </param>
    /// <param name="fadeSpeed">
    ///     Coeficient to multiply fading speed by. 1.0 speed is 300ms. Set to 0 for no fade.
    /// </param>
    public async void PlayMusic(string path, float fadeSpeed = 0.0f, bool loop = true)
    {
        musicFade.SpeedScale = fadeSpeed == 0.0f ? 32 : fadeSpeed;

        musicFade.Stop();
        musicFade.Play("FadeOut");
        await ToSignal(musicFade, "animation_finished");
        AudioStreamOggVorbis loadedAudio;
        if (path.StartsWith("res://"))
        {
            loadedAudio = GD.Load<AudioStreamOggVorbis>(path);
        }
        else
        {
            loadedAudio = GD.Load<AudioStreamOggVorbis>("res://Audio/" + path);
        }
        loadedAudio.Loop = loop;
        music.Stream = loadedAudio;
        musicFade.Play("FadeIn");
    }

    public void StopMusic(float fadeSpeed = 1.0f)
    {
        musicFade.Stop();
        musicFade.SpeedScale = fadeSpeed == 0 ? 32 : fadeSpeed;
        musicFade.Play("FadeOut");
    }

    /// <summary>
    /// path relative to "res://Audio", eg. path = "Music/Song.ogg" will load "res://Audio/Music/Song.ogg"
    /// </summary>
    public async void PlayAmbient(string path, float fadeSpeed = 1)
    {
        ambientFade.SpeedScale = fadeSpeed;

        ambientFade.Stop();
        ambientFade.Play("FadeOut");
        await ToSignal(ambientFade, "animation_finished");

        AudioStreamOggVorbis loadedAudio = GD.Load<AudioStreamOggVorbis>("res://Audio/" + path);
        loadedAudio.Loop = true;
        ambient.Stream = loadedAudio;
        ambientFade.Play("FadeIn");
    }

    public void StopAmbient(float fadeSpeed = 1)
    {
        ambientFade.Stop();
        ambientFade.SpeedScale = fadeSpeed == 0 ? 32 : fadeSpeed;
        ambientFade.Play("FadeOut");
    }
}
