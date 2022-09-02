using Godot;
using System;

public class AudioEngine : AudioStreamPlayer
{
    AnimationPlayer fade;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        fade = GetNode<AnimationPlayer>("Fade");
    }

    /// <summary>
    /// Eg. path = "Music/Song.ogg" will load "res://Audio/Music/Song.ogg"
    /// </summary>
    public async void FadeMusic (string path, float fadeSpeed = 1, bool loop = true)
    {
        fade.PlaybackSpeed = fadeSpeed;

        fade.Play("FadeOut");
        await ToSignal(fade, "animation_finished");

        Stop();
        AudioStreamOGGVorbis loadedAudio = GD.Load<AudioStreamOGGVorbis>("res://Audio/" + path);
        loadedAudio.Loop = loop;
        Stream = loadedAudio;

        fade.Play("FadeIn");
        Play();

    }
}
