using UnityEngine;
using SJ.Audio;

namespace SJ.UI
{
    public class SoundMenu : SJMonoBehaviour
    {

        [SerializeField]
        private SoundSlider general, music, effects;

        private ISoundService soundService;

        protected override void SJAwake()
        {
            soundService = Application.GetSoundService();

            general.SetData(ChangeGeneralVolume, soundService.GetVolume());
            music.SetData(ChangeMusicChannelVolume, soundService.GetVolumeOfChannel(SoundChannels.Music));
            effects.SetData(ChangeEffectsChannelVolume, soundService.GetVolumeOfChannel(SoundChannels.Effects));

        }

        private void ChangeGeneralVolume(float value)
        {
            soundService.SetVolume(value);

            ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();

            gameConfiguration.generalVolume = soundService.GetVolume();

            GameConfigurationCareTaker.SaveConfiguration();
        }

        private void ChangeMusicChannelVolume(float value)
        {
            soundService.SetVolumeOfChannel(SoundChannels.Music, value);

            ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();
            
            gameConfiguration.musicVolume = soundService.GetVolumeOfChannel(SoundChannels.Music);

            GameConfigurationCareTaker.SaveConfiguration();
        }

        private void ChangeEffectsChannelVolume(float value)
        {
            soundService.SetVolumeOfChannel(SoundChannels.Effects, value);

            ref GameConfiguration gameConfiguration = ref GameConfigurationCareTaker.GetConfiguration();
            
            gameConfiguration.soundsVolume = soundService.GetVolumeOfChannel(SoundChannels.Effects);

            GameConfigurationCareTaker.SaveConfiguration();
        }
    }
}


