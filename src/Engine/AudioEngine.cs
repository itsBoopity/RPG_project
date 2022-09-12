using Godot;
using System;

public class AudioEngine : Node
{
    private AudioStreamPlayer music;
    private AudioStreamPlayer ambient;
    private AnimationPlayer fade;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        music = GetNode<AudioStreamPlayer>("Music");
        ambient = GetNode<AudioStreamPlayer>("Ambient");
        fade = GetNode<AnimationPlayer>("Fade");
    }

    /// <summary>
    /// path relative to "res://Audio", eg. path = "Music/Song.ogg" will load "res://Audio/Music/Song.ogg"
    /// </summary>
    public async void PlayMusic(string path, float fadeSpeed = 1, bool loop = true)
    {
        fade.PlaybackSpeed = fadeSpeed == 0 ? 32 : fadeSpeed;

        fade.Stop();
        fade.Play("FadeOut");
        await ToSignal(fade, "animation_finished");

        AudioStreamOGGVorbis loadedAudio = GD.Load<AudioStreamOGGVorbis>("res://Audio/" + path);
        loadedAudio.Loop = loop;
        music.Stream = loadedAudio;
        fade.Play("FadeIn");
    }

    public void StopMusic(float fadeSpeed = 1)
    {
        fade.Stop();
        fade.PlaybackSpeed = fadeSpeed == 0 ? 32 : fadeSpeed;
        fade.Play("FadeOut");
    }

    /// <summary>
    /// path relative to "res://Audio", eg. path = "Music/Song.ogg" will load "res://Audio/Music/Song.ogg"
    /// </summary>
    public async void PlayAmbient(string path, float fadeSpeed = 1)
    {
        fade.PlaybackSpeed = fadeSpeed;

        fade.Stop();
        fade.Play("FadeOutAmbient");
        await ToSignal(fade, "animation_finished");

        AudioStreamOGGVorbis loadedAudio = GD.Load<AudioStreamOGGVorbis>("res://Audio/" + path);
        loadedAudio.Loop = true;
        ambient.Stream = loadedAudio;
        fade.Play("FadeInAmbient");
    }

    public void StopAmbient(float fadeSpeed = 1)
    {
        fade.Stop();
        fade.PlaybackSpeed = fadeSpeed == 0 ? 32 : fadeSpeed;
        fade.Play("FadeOutAmbient");
    }


}
