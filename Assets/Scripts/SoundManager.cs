using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum AudioRef
{
    Music,
    Ambient,
    Click,
    Chime,
    Door
}

[Serializable]
public struct AudioItem
{
    public AudioRef AudioRef;
    public AudioClip AudioClip;
    public float Volume;
}

public struct BufferedAudioSource
{
    public AudioSource[] Sources;
    public int LastIdx;
}

public class SoundManager : MonoBehaviour
{
    public AudioItem[] MusicItems;
    public AudioItem[] SfxItems;

    private Dictionary<AudioRef, AudioSource> _musicClips = new();
    private Dictionary<AudioRef, BufferedAudioSource> _sfxClips = new();
    
    private Dictionary<AudioRef, float> _volumes = new();

    public void Init()
    {
    // TODO volumes
        foreach (var musicItem in MusicItems)
        {
            _musicClips.Add(musicItem.AudioRef, CreateAudioSource(musicItem, true));
            _volumes.Add(musicItem.AudioRef, musicItem.Volume);
        }
        foreach (var sfxItem in SfxItems)
        {
            _sfxClips.Add(sfxItem.AudioRef, CreateBufferedAudioSource(sfxItem));
            _volumes.Add(sfxItem.AudioRef, sfxItem.Volume);
        }
    }

    private AudioSource CreateAudioSource(AudioItem audioItem, bool loop)
    {
        var go = new GameObject(audioItem.AudioRef.ToString());
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        var src = go.AddComponent<AudioSource>();
        src.clip = audioItem.AudioClip;
        src.playOnAwake = loop;
        src.loop = loop;
        src.volume = loop ? 0 : audioItem.Volume;
        return src;
    }
    
    private BufferedAudioSource CreateBufferedAudioSource(AudioItem audioItem, int bufferLen = 2)
    {
        var bufSrc = new BufferedAudioSource();
        bufSrc.Sources = new AudioSource[bufferLen];
        for (var i = 0; i < bufSrc.Sources.Length; i++)
        {
            bufSrc.Sources[i] = CreateAudioSource(audioItem, false);
        }
        
        return bufSrc;
    }

    public void PlayMusic(AudioRef audioRef)
    {
        var src = _musicClips[audioRef];
        src.volume = _volumes[audioRef];
    }
    
    public void DoPlayMusic(AudioRef audioRef, float fadeTime)
    {
        var src = _musicClips[audioRef];
        var volume = _volumes[audioRef];
        src.Play();
        src.volume = 0;
        src.DOKill();
        src.DOFade(volume, fadeTime);
    }

    public void PauseMusic(AudioRef audioRef)
    {
        var src = _musicClips[audioRef];
        src.Pause();
    }

    public void DoPauseMusic(AudioRef audioRef, float fadeTime)
    {
        var src = _musicClips[audioRef];
        src.DOKill();
        src.DOFade(0, fadeTime);
    }

    public void PlaySound(AudioRef audioRef)
    {
        var src = _sfxClips[audioRef];
        var idx = src.LastIdx;
        src.Sources[idx].Play();
        ++idx;
        idx %= src.Sources.Length;
        src.LastIdx = idx;
    }
}
