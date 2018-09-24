using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SJAudioSource : SJMonoBehaviour {

    [SerializeField]
    protected AudioSource source;
    [SerializeField]
    private float independentVolume = 1;
    [SerializeField]
    private bool playOnStart;

    public AudioSource Source
    {
        get
        {
            return source;
        }

        protected set
        {
            source = value;
        }
    }

    public AudioClip Clip
    {
        get
        {
            return source.clip;
        }

        set
        {
            source.clip = value;
        }
    }

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

                UpdateScaledVolume(soundChannel.ScaledVolume);
            }
            else
            {
                EditorDebug.Log("El valor del volumen debe ser entre 0 y 1");
            }
        }
    }

    public float ScaledVolume
    {
        get
        {
            return source.volume;
        }
    }

    public bool Loop
    {
        get
        {
            return source.loop;
        }

        set
        {
            source.loop = value;
        }
    }

    public bool IsPlaying
    {
        get
        {
            return Source.isPlaying;
        }
    }

    protected SoundManager soundManager;

    [SerializeField]
    protected SoundManager.SoundChannels channel;
    protected SoundChannel soundChannel;

    public event ScaledVolumeChanged onVolumeChanged;

    protected ScaledVolumeChanged scaledVolumeChangedDelegate;

    protected void Awake()
    {
        soundManager = SoundManager.Instance;

        soundChannel = soundManager.Channels[channel];

        IndependentVolume = independentVolume;

        scaledVolumeChangedDelegate = OnChannelVolumeChanged;

        soundChannel.onVolumeChanged += scaledVolumeChangedDelegate;

        soundManager.AddSource(this);
    }

    protected void Start()
    {
        if(playOnStart)
        {
            Play();
        }
    }

    private void OnDestroy()
    {
        soundChannel.onVolumeChanged -= scaledVolumeChangedDelegate;
    }

    public void PlayOneShot(AudioClip clip)
    {
        Source.PlayOneShot(clip);
    }

    public void PlayOneShot(AudioClip clip, float volumeScale)
    {
        Source.PlayOneShot(clip, volumeScale);
    }

    public void PlayOneShotAtPosition(Vector3 position, AudioClip clip)
    {
        gameObject.transform.position = position;

        Source.PlayOneShot(clip);
    }

    public void PlayOneShotAtPosition(Vector3 position, AudioClip clip, float volumeScale)
    {
        gameObject.transform.position = position;

        Source.PlayOneShot(clip, volumeScale);
    }

    public void PlayAtPosition(Vector3 position)
    {
        gameObject.transform.position = position;

        Source.Play();
    }

    public void Play()
    {
        Source.Play();
    }

    public void PlayAtPosition(Vector3 position, AudioClip clip)
    {
        Clip = clip;

        PlayAtPosition(position);
    }

    public void ChangeVolume(float volume)
    {
        IndependentVolume = volume;
    }

    private void OnChannelVolumeChanged(float independentVolume, float scaledVolume)
    {
        UpdateScaledVolume(scaledVolume);
    }

    protected void UpdateScaledVolume(float dependencyVolume)
    {
        Source.volume = IndependentVolume * dependencyVolume;

        if (onVolumeChanged != null)
        {
            onVolumeChanged(IndependentVolume, ScaledVolume);
        }
    }

    public float GetVolume()
    {
        return IndependentVolume;
    }
}
