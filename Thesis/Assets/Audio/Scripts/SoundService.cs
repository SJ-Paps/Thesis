using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

namespace SJ.Audio
{
    public sealed class SoundService : ISoundService
    {
        private class SoundChannel
        {
            private float independentVolume = 1;

            public float IndependentVolume
            {
                get
                {
                    return independentVolume;
                }

                set
                {
                    independentVolume = value;

                    UpdateScaledVolume(soundManager.GetVolume());
                }
            }

            public float ScaledVolume { get; private set; }

            public event ChannelVolumeChanged onVolumeChanged;

            private SoundService soundManager;

            public SoundChannel(SoundService soundManager)
            {
                this.soundManager = soundManager;

                soundManager.onGlobalVolumeChanged += UpdateScaledVolume;

                UpdateScaledVolume(soundManager.GetVolume());
            }

            private void UpdateScaledVolume(float dependencyVolume)
            {
                ScaledVolume = dependencyVolume * IndependentVolume;
            }

        }

        private float volume = 1;

        private SJAudioSource audioSourcePrefab;
        private List<SJAudioSource> audioSourcesPool;


        private Dictionary<SoundChannels, SoundChannel> channels;

        public event Action<float> onGlobalVolumeChanged;
        public event ChannelVolumeChanged onChannelVolumeChanged;

        public SoundService()
        {
            channels = new Dictionary<SoundChannels, SoundChannel>();

            channels.Add(SoundChannels.Effects, new SoundChannel(this));
            channels.Add(SoundChannels.Music, new SoundChannel(this));

            audioSourcesPool = new List<SJAudioSource>();
            audioSourcePrefab = SJResources.LoadComponentOfGameObject<SJAudioSource>("SJAudioSourcePrefab");

            SJMonoBehaviour.OnInstantiation += OnObjectInstantiated;
            SJMonoBehaviour.OnDestruction += OnObjectDestroyed;
        }

        private void OnObjectInstantiated(SJMonoBehaviour obj)
        {
            if (obj is SJAudioSource audioSource)
            {
                AddSource(audioSource);
            }
        }

        private void OnObjectDestroyed(SJMonoBehaviour obj)
        {
            if (obj is SJAudioSource audioSource)
            {
                RemoveSource(audioSource);
            }
        }

        private void AddSource(SJAudioSource source)
        {
            if (audioSourcesPool.Contains(source) == false)
            {
                audioSourcesPool.Add(source);
            }
        }

        private void RemoveSource(SJAudioSource source)
        {
            audioSourcesPool.Remove(source);
        }

        public float GetVolume()
        {
            return volume;
        }

        public void SetVolume(float value)
        {
            if(volume != value)
            {
                volume = value;

                if (volume < 0)
                {
                    volume = 0;
                }
                else if (volume > 1)
                {
                    volume = 1;
                }

                if (onGlobalVolumeChanged != null)
                {
                    onGlobalVolumeChanged(volume);
                }
            }
        }

        private SJAudioSource GetFirstAvailable()
        {
            for (int i = 0; i < audioSourcesPool.Count; i++)
            {
                if (audioSourcesPool[i].IsPlaying == false)
                {
                    return audioSourcesPool[i];
                }
            }

            SJAudioSource newSource = GameObject.Instantiate<SJAudioSource>(audioSourcePrefab);

            audioSourcesPool.Add(newSource);

            return newSource;
        }

        public int Play(AudioClip audioClip)
        {
            SJAudioSource audioSource = GetFirstAvailable();

            int id = audioSourcesPool.IndexOf(audioSource);

            audioSource.Clip = audioClip;

            audioSource.Play();

            return id;
        }

        public int PlayAtPosition(Vector3 position, AudioClip audioClip)
        {
            return Play(audioClip);
        }

        public void Stop(int id)
        {
            if(ContainsIndex(id))
            {
                audioSourcesPool[id].Stop();
            }
        }

        private bool ContainsIndex(int index)
        {
            return index > -1 && index < audioSourcesPool.Count;
        }

        public void SetVolumeOfChannel(SoundChannels soundChannel, float volume)
        {
            var channelObject = channels[soundChannel];

            if (channelObject.IndependentVolume != volume)
            {
                channelObject.IndependentVolume = volume;

                if(onChannelVolumeChanged != null)
                {
                    onChannelVolumeChanged(soundChannel, channelObject.IndependentVolume, channelObject.ScaledVolume);
                }
            }
        }

        public float GetVolumeOfChannel(SoundChannels soundChannel)
        {
            return channels[soundChannel].IndependentVolume;
        }

        public float GetScaledVolumeOfChannel(SoundChannels soundChannel)
        {
            return channels[soundChannel].ScaledVolume;
        }
    }

    
}




