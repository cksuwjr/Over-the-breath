using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static private GameObject container;
    static private SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "Sound Manager";
                instance = container.AddComponent(typeof(SoundManager)) as SoundManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    public AudioSource effectSoundSource;
    public AudioSource BackgroundSoundSource;
    private AudioListener soundListener;

    public List<AudioClip> backgroundMusics;
    public List<AudioClip> effectSounds;

    private void Awake()
    {
        if(effectSoundSource == null)
            effectSoundSource = gameObject.AddComponent<AudioSource>();
        if (BackgroundSoundSource == null)
        {
            BackgroundSoundSource = gameObject.AddComponent<AudioSource>();
            BackgroundSoundSource.loop = true;
        }
        if(soundListener == null)
            soundListener = gameObject.AddComponent<AudioListener>();

        container = this.gameObject;
        instance = this;
        DontDestroyOnLoad(container);
    }

    public void Sound(string clipName, float pitch = 1f, float volume = 1f)
    {
        if (effectSounds == null) return;

        foreach(AudioClip clip in effectSounds)
        {
            if (clip.name == clipName)
            {
                //Debug.Log("찾아서 플레이합니다 효과음-" + clipName);
                effectSoundSource.clip = clip;
                effectSoundSource.pitch = pitch;
                //effectSoundSource.volume = volume;
                effectSoundSource.Play();
                break;
            }
        }
    }
    
    public void ChangeBackgroundMusic(string clipName, float pitch = 1f, float volume = 1f)
    {
        foreach (AudioClip clip in backgroundMusics)
        {
            if (clip.name == clipName)
            {
                if(BackgroundSoundSource.clip != null)
                    if (clipName == BackgroundSoundSource.clip.name.ToString() && BackgroundSoundSource.pitch == pitch && BackgroundSoundSource.volume == volume)
                        return;

                //Debug.Log("찾아서 플레이합니다 음악-" + clipName);
                BackgroundSoundSource.clip = clip;
                BackgroundSoundSource.pitch = pitch;
                //BackgroundSoundSource.volume = volume;
                BackgroundSoundSource.Play();
                break;
            }
        }
    }


}
