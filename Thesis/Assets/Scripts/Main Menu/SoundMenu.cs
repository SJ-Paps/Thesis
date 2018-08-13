using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SoundMenu : MonoBehaviour {

    [SerializeField]
    private SoundSlider general, music, effects;

    private SoundManager soundManager;
    
    void Awake () {

        soundManager = SoundManager.Instance;
        
        general.SetData(soundManager.ChangeVolume, soundManager.Volume);
        music.SetData(soundManager.Channels[SoundManager.SoundChannels.Music].ChangeVolume, soundManager.Channels[SoundManager.SoundChannels.Music].IndependentVolume);
        effects.SetData(soundManager.Channels[SoundManager.SoundChannels.Effects].ChangeVolume, soundManager.Channels[SoundManager.SoundChannels.Effects].IndependentVolume);

	}
}
