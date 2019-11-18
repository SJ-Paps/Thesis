using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SJ.Audio
{
    public delegate void ChannelVolumeChanged(SoundChannels soundChannel, float independentVolume, float scaledVolume);

    public interface ISoundService
    {
        event Action<float> onGlobalVolumeChanged;
        event ChannelVolumeChanged onChannelVolumeChanged;

        int Play(AudioClip audioClip);
        void Stop(int id);

        int PlayAtPosition(Vector3 position, AudioClip clip);

        void SetVolume(float volume);
        void SetVolumeOfChannel(SoundChannels soundChannel, float volume);
        
        float GetVolume();
        float GetVolumeOfChannel(SoundChannels soundChannel);
        float GetScaledVolumeOfChannel(SoundChannels soundChannel);
    }
}


