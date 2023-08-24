using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : SingletonPersistent<AudioManager>
{
    public float SfxVolume => _sfxSource.volume;
    public float MusicVolume => _musicSource.volume;

    [SerializeField] Sound[] _soundEffects;
    [SerializeField] Sound[] _musicTracks;
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioSource _musicSource;

    public void PlaySFX(SoundType soundType)
    {
        Sound sound = Array.Find(_soundEffects, s => s.Name == soundType);

        if (sound == null)
        {
            Debug.LogWarning("Sound not found for sound type: " + soundType);
            return;
        }

        if (_sfxSource == null)
        {
            Debug.LogWarning("AudioSource not assigned to AudioManager.");
            return;
        }
        _sfxSource.PlayOneShot(sound.Clip);
    }

    public void PlayMusic(SoundType soundType)
    {
        Sound sound = Array.Find(_soundEffects, s => s.Name == soundType);
        _musicSource.clip = sound.Clip;
        _musicSource.Play();
    }

    public void ToggleSFX() => _sfxSource.mute = !_sfxSource.mute;

    public void ToggleMusic() => _musicSource.mute = !_musicSource.mute;

    public void ChangeSFXVolume(float volume) => _sfxSource.volume = volume;

    public void ChangeMusicVolume(float volume) => _musicSource.volume = volume;
}
