using UnityEngine;
using System;

namespace SJ.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SJAudioSource : SJMonoBehaviour
    {
        public delegate void VolumeChanged(float independentVolume, float scaledVolume);

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

                    UpdateScaledVolume(SoundService.GetScaledVolumeOfChannel(channel));
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

        private ISoundService soundServiceField;

        protected ISoundService SoundService
        {
            get
            {
                if(soundServiceField == null)
                {
                    soundServiceField = Application.SoundService;
                }

                return soundServiceField;
            }
        }

        [SerializeField]
        protected SoundChannels channel;

        public SoundChannels Channel { get { return channel; } }

        public event VolumeChanged onVolumeChanged;

        protected override void SJAwake()
        {
            IndependentVolume = independentVolume;

            SoundService.onChannelVolumeChanged += OnChannelVolumeChanged;
            SoundService.onGlobalVolumeChanged += OnGlobalVolumeChanged;
        }

        protected override void SJStart()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        protected override void SJOnDestroy()
        {
            SoundService.onChannelVolumeChanged -= OnChannelVolumeChanged;
        }

        private void OnChannelVolumeChanged(SoundChannels soundChannel, float independentVolume, float scaledVolume)
        {
            if(channel == soundChannel)
            {
                UpdateScaledVolume(scaledVolume);
            }
        }

        private void OnGlobalVolumeChanged(float volume)
        {
            UpdateScaledVolume(SoundService.GetScaledVolumeOfChannel(channel));
        }

        public void Play()
        {
            Source.Play();
        }

        public void Stop()
        {
            Source.Stop();
        }

        protected void UpdateScaledVolume(float dependencyVolume)
        {
            Source.volume = IndependentVolume * dependencyVolume;

            if (onVolumeChanged != null)
            {
                onVolumeChanged(IndependentVolume, ScaledVolume);
            }
        }
    }

}

