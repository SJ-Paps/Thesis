using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void ScaledVolumeChanged(float independentVolume, float scaledVolume);

public sealed class SoundManager
{


    public enum SoundChannels : byte
    {
        Effects,
        Music,
        Count
    }

    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SoundManager();
            }

            return instance;
        }
    }

    private float volume = 1;

    public float Volume
    {
        get
        {
            return volume;
        }

        set
        {
            if (value >= 0 && value <= 1)
            {
                volume = value;

                if (onVolumeChanged != null)
                {
                    onVolumeChanged(volume);
                }
            }
            else
            {
                Logger.LogConsole("El valor del volumen debe ser entre 0 y 1");
            }
        }
    }

    private SJAudioSource audioSourcePrefab;
    private List<SJAudioSource> audioSourcesPool;
    private const int initialPoolSize = 10;


    private Dictionary<SoundChannels, SoundChannel> channels;

    public Dictionary<SoundChannels, SoundChannel> Channels
    {
        get
        {
            return channels;
        }
    }

    public event Action<float> onVolumeChanged;

    private SoundManager()
    {
        channels = new Dictionary<SoundChannels, SoundChannel>();

        for(SoundChannels i = 0; i < SoundChannels.Count; i++)
        {
            channels.Add(i, new SoundChannel(this));
        }

        audioSourcesPool = new List<SJAudioSource>();
        audioSourcePrefab = SJResources.LoadAsset<GameObject>("SJAudioSourcePrefab").GetComponent<SJAudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void SetInitialPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddSource(GameObject.Instantiate<SJAudioSource>(audioSourcePrefab));
        }
    }

    public void AddSource(SJAudioSource source)
    {
        if(audioSourcesPool.Contains(source) == false)
        {
            audioSourcesPool.Add(source);
        }
    }

    public void ChangeVolume(float volume)
    {
        Volume = volume;
    }

    public void PlayOneShotAtPosition(Vector3 position, AudioClip clip)
    {
        SJAudioSource source = GetFirstAvailable();

        source.PlayOneShotAtPosition(position, clip);
    }

    public void PlayOneShot(AudioClip clip)
    {
        SJAudioSource source = GetFirstAvailable();

        source.PlayOneShot(clip);
    }

    private SJAudioSource GetFirstAvailable()
    {
        for(int i = 0; i < audioSourcesPool.Count; i++)
        {
            if(audioSourcesPool[i].IsPlaying == false)
            {
                return audioSourcesPool[i];
            }
        }

        SJAudioSource newSource = GameObject.Instantiate<SJAudioSource>(audioSourcePrefab);

        audioSourcesPool.Add(newSource);

        return newSource;
    }

    public float GetVolume()
    {
        return Volume;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetInitialPool();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        for(int i = 0; i < audioSourcesPool.Count; i++)
        {
            GameObject.Destroy(audioSourcesPool[i]);
        }

        audioSourcesPool.Clear();
    }
}

public class SoundChannel
{
    private float independentVolume = 1;
    private float scaledVolume;

    public float IndependentVolume
    {
        get
        {
            return independentVolume;
        }

        set
        {
            if (value >= 0 && value <= 1)
            {
                independentVolume = value;

                UpdateScaledVolume(soundManager.Volume);

                if (onVolumeChanged != null)
                {
                    onVolumeChanged(IndependentVolume, ScaledVolume);
                }
            }
            else
            {
                Logger.LogConsole("El valor del volumen debe ser entre 0 y 1");
            }
        }
    }

    public float ScaledVolume
    {
        get
        {
            return scaledVolume;
        }
    }

    public event ScaledVolumeChanged onVolumeChanged;

    private SoundManager soundManager;

    public SoundChannel(SoundManager soundManager)
    {
        this.soundManager = soundManager;

        soundManager.onVolumeChanged += UpdateScaledVolume;

        UpdateScaledVolume(soundManager.Volume);
    }

    private void UpdateScaledVolume(float dependencyVolume)
    {
        scaledVolume = dependencyVolume * IndependentVolume;

        if(onVolumeChanged != null)
        {
            onVolumeChanged(IndependentVolume, ScaledVolume);
        }
    }

    public void ChangeVolume(float volume)
    {
        IndependentVolume = volume;
    }

    public float GetVolume()
    {
        return IndependentVolume;
    }

}


