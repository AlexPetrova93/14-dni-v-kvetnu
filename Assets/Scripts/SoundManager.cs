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

    public void Init()
    {
        foreach (var musicItem in MusicItems)
        {
            _musicClips.Add(musicItem.AudioRef, CreateAudioSource(musicItem));
        }
        foreach (var sfxItem in SfxItems)
        {
            _sfxClips.Add(sfxItem.AudioRef, CreateBufferedAudioSource(sfxItem));
        }
    }

    private AudioSource CreateAudioSource(AudioItem audioItem)
    {
        var go = new GameObject(audioItem.AudioRef.ToString());
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        var src = go.AddComponent<AudioSource>();
        src.clip = audioItem.AudioClip;
        src.playOnAwake = false;
        return src;
    }
    
    private BufferedAudioSource CreateBufferedAudioSource(AudioItem audioItem, int bufferLen = 2)
    {
        var bufSrc = new BufferedAudioSource();
        bufSrc.Sources = new AudioSource[bufferLen];
        for (var i = 0; i < bufSrc.Sources.Length; i++)
        {
            bufSrc.Sources[i] = CreateAudioSource(audioItem);
        }
        
        return bufSrc;
    }

    public void PlayMusic(AudioRef audioRef)
    {
        var src = _musicClips[audioRef];
        src.Play();
    }
    
    public void DoPlayMusic(AudioRef audioRef, float fadeTime)
    {
        var src = _musicClips[audioRef];
        src.Play();
        src.volume = 0;
        src.DOKill();
        src.DOFade(1, fadeTime);
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
