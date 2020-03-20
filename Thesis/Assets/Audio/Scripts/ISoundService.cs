using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SJ.Audio
{
    public delegate void ChannelVolumeChanged(SoundChannels soundChannel, float independentVolume, float scaledVolume);

    public interface ISoundService
    {
        event Action<float> OnGlobalVolumeChanged;
        event ChannelVolumeChanged OnChannelVolumeChanged;

        void AddAudioSource(SJAudioSource audioSource);
        void RemoveAudioSource(SJAudioSource audioSource);

        string Play(AudioClip audioClip);
        void Stop(string guid);

        void PlayOneShot(AudioClip audioClip);

        string PlayAtPosition(Vector3 position, AudioClip clip);

        void SetVolume(float volume);
        void SetVolumeOfChannel(SoundChannels soundChannel, float volume);
        
        float GetVolume();
        float GetVolumeOfChannel(SoundChannels soundChannel);
        float GetScaledVolumeOfChannel(SoundChannels soundChannel);
    }
}


