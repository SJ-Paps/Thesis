using UnityEngine;

public class SoundMenu : MonoBehaviour {

    [SerializeField]
    private SoundSlider general, music, effects;
    
    void Awake ()
    {
        GameConfiguration gameConfiguration = GameConfigurationCareTaker.GetConfiguration();
        
        general.SetData(ChangeGeneralVolume, SoundManager.GetInstance().Volume);
        music.SetData(ChangeMusicChannelVolume, SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Music].IndependentVolume);
        effects.SetData(ChangeEffectsChannelVolume, SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Effects].IndependentVolume);

	}

    private void ChangeGeneralVolume(float value)
    {
        SoundManager.GetInstance().ChangeVolume(value);

        ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

        gameConfiguration.generalVolume = SoundManager.GetInstance().Volume;

        GameConfigurationCareTaker.SaveConfiguration();
    }

    private void ChangeMusicChannelVolume(float value)
    {
        SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Music].ChangeVolume(value);

        ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

        gameConfiguration.musicVolume = SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Music].IndependentVolume;

        GameConfigurationCareTaker.SaveConfiguration();
    }

    private void ChangeEffectsChannelVolume(float value)
    {
        SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Effects].ChangeVolume(value);

        ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

        gameConfiguration.soundsVolume = SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Effects].IndependentVolume;

        GameConfigurationCareTaker.SaveConfiguration();
    }
}
