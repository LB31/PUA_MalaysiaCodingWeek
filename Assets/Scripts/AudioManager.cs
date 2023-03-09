using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public List<Audio> audioClips = new();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySound(NonSpatialSound sound)
    {
        Audio audio = audioClips.FirstOrDefault(x => x.Sound == sound);
        if (audio == null) return;

        audioSource.clip = audio.AudioClip;
        audioSource.Play();
    }
}

public enum NonSpatialSound
{
    Click,
    Enlarge,
    Shrink,
    Move
}
[Serializable]
public class Audio
{
    public NonSpatialSound Sound;
    public AudioClip AudioClip;
}
