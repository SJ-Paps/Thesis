using UnityEngine;
using Application = SJ.Management.Application;

namespace SJ.Management.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SJAudioSource : SJMonoBehaviour
    {
        public delegate void VolumeChanged(float independentVolume, float scaledVolume);

        [Range(0f, 1f)]
        [SerializeField]
        private float independentVolume = 1;
        [SerializeField]
        private bool playOnStart;

        private AudioSource innerSource;

        private AudioSource InnerSource
        {
            get
            {
                if (innerSource == null)
                    innerSource = GetComponent<AudioSource>();

                return innerSource;
            }
        }

        public AudioClip Clip { get => InnerSource.clip; set => InnerSource.clip = value; }

        public float IndependentVolume
        {
            get => independentVolume;

            set
            {
                if (value >= 0 && value <= 1)
                {
                    independentVolume = value;

                    UpdateScaledVolume(SoundService.GetScaledVolumeOfChannel(channel));
                }
                else
                    Logger.LogConsole("value must be between 0 and 1");
            }
        }

        public float ScaledVolume => InnerSource.volume;

        public bool Loop { get => InnerSource.loop; set => InnerSource.loop = value; }

        public bool IsPlaying => InnerSource.isPlaying;

        protected ISoundService SoundService => Application.Instance.SoundService();

        [SerializeField]
        protected SoundChannels channel;

        public SoundChannels Channel { get { return channel; } }

        public event VolumeChanged OnVolumeChanged;

        protected override void SJAwake()
        {
            SoundService.AddAudioSource(this);
            UpdateScaledVolume(SoundService.GetScaledVolumeOfChannel(channel));

            SoundService.OnChannelVolumeChanged += OnChannelVolumeChanged;
            SoundService.OnGlobalVolumeChanged += OnGlobalVolumeChanged;
        }

        protected override void SJStart()
        {
            if (playOnStart)
                Play();
        }

        protected override void SJOnDestroy()
        {
            SoundService.RemoveAudioSource(this);

            SoundService.OnChannelVolumeChanged -= OnChannelVolumeChanged;
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
            InnerSource.Play();
        }

        public void Stop()
        {
            InnerSource.Stop();
        }

        public void PlayOneShot(AudioClip clip)
        {
            InnerSource.PlayOneShot(clip);
        }

        protected void UpdateScaledVolume(float dependencyVolume)
        {
            InnerSource.volume = IndependentVolume * dependencyVolume;

            OnVolumeChanged?.Invoke(IndependentVolume, ScaledVolume);
        }
    }
}

