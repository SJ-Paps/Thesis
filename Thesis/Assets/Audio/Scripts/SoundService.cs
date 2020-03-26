using Paps.Unity;
using SJ.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

                soundManager.OnGlobalVolumeChanged += UpdateScaledVolume;

                UpdateScaledVolume(soundManager.GetVolume());
            }

            private void UpdateScaledVolume(float dependencyVolume)
            {
                ScaledVolume = dependencyVolume * IndependentVolume;
            }

        }

        private float volume = 1;

        private SJAudioSource audioSourcePrefab;
        private Dictionary<string, SJAudioSource> audioSourcesPool;

        private Dictionary<SoundChannels, SoundChannel> channels;

        private SJAudioSource playOneShotSource;

        public event Action<float> OnGlobalVolumeChanged;
        public event ChannelVolumeChanged OnChannelVolumeChanged;

        public SoundService(float generalVolume, float musicVolume, float effectsVolume)
        {
            channels = new Dictionary<SoundChannels, SoundChannel>();

            channels.Add(SoundChannels.Effects, new SoundChannel(this));
            channels.Add(SoundChannels.Music, new SoundChannel(this));

            SetVolume(generalVolume);
            SetVolumeOfChannel(SoundChannels.Music, musicVolume);
            SetVolumeOfChannel(SoundChannels.Effects, effectsVolume);

            audioSourcesPool = new Dictionary<string, SJAudioSource>();
            audioSourcePrefab = SJResources.LoadComponentOfGameObject<SJAudioSource>("SJAudioSourcePrefab");
        }

        public void AddAudioSource(SJAudioSource source)
        {
            AddAudioSource(source, out string _);
        }

        private void AddAudioSource(SJAudioSource source, out string guid)
        {
            guid = null;

            if (audioSourcesPool.Values.Contains(source) == false)
            {
                guid = Guid.NewGuid().ToString();
                audioSourcesPool.Add(guid, source);
            }
        }

        public void RemoveAudioSource(SJAudioSource source)
        {
            foreach(var audioSource in audioSourcesPool)
            {
                if (audioSource.Value == source)
                {
                    audioSourcesPool.Remove(audioSource.Key);
                    return;
                }
            }
        }

        public float GetVolume() => volume;

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

                OnGlobalVolumeChanged?.Invoke(volume);
            }
        }

        private string GetFirstAvailable()
        {
            foreach(var audioSource in audioSourcesPool)
            {
                if (audioSource.Value.IsPlaying == false)
                    return audioSource.Key;
            }

            SJAudioSource newSource = GameObject.Instantiate<SJAudioSource>(audioSourcePrefab);

            AddAudioSource(newSource, out string guid);

            return guid;
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            if (playOneShotSource == null)
                InstantiatePlayOneShotAudioSource();

            playOneShotSource.PlayOneShot(audioClip);
        }

        public string Play(AudioClip audioClip)
        {
            var guid = GetFirstAvailable();

            var audioSource = audioSourcesPool[guid];

            audioSource.Clip = audioClip;

            audioSource.Play();

            return guid;
        }

        public string PlayAtPosition(Vector3 position, AudioClip audioClip)
        {
            var guid = GetFirstAvailable();

            var audioSource = audioSourcesPool[guid];

            audioSource.Clip = audioClip;

            audioSource.gameObject.transform.position = position;

            audioSource.Play();

            return guid;
        }

        public void Stop(string guid)
        {
            if (audioSourcesPool.ContainsKey(guid))
                audioSourcesPool[guid].Stop();
        }

        public void SetVolumeOfChannel(SoundChannels soundChannel, float volume)
        {
            var channelObject = channels[soundChannel];

            if (channelObject.IndependentVolume != volume)
            {
                channelObject.IndependentVolume = volume;

                OnChannelVolumeChanged?.Invoke(soundChannel, channelObject.IndependentVolume, channelObject.ScaledVolume);
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

        private void InstantiatePlayOneShotAudioSource()
        {
            playOneShotSource = GameObject.Instantiate(audioSourcePrefab);
            UnityUtil.DontDestroyOnLoad(playOneShotSource);
        }
    }
}