using UnityEngine;
using SJ.Audio;
using System.Threading.Tasks;

namespace SJ.UI
{
    public class SoundMenu : SJMonoBehaviour
    {
        [SerializeField]
        private SoundSlider general, music, effects;

        private ISoundService soundService;
        private IGameSettingsRepository gameSettingsRepository;

        protected override void SJAwake()
        {
            soundService = Application.GetSoundService();
            gameSettingsRepository = Repositories.GetGameSettingsRepository();

            general.onValueChanged += OnGeneralVolumeSliderValueChanged;
            general.onDragEnd += OnGeneralVolumeSliderDragEnd;
            general.SetValue(soundService.GetVolume());

            music.onValueChanged += OnMusicSliderValueChanged;
            music.onDragEnd += OnMusicSliderDragEnd;
            music.SetValue(soundService.GetVolumeOfChannel(SoundChannels.Music));

            effects.onValueChanged += OnEffectsSliderValueChanged;
            effects.onDragEnd += OnEffectsSliderDragEnd;
            effects.SetValue(soundService.GetVolumeOfChannel(SoundChannels.Effects));

        }

        private void OnGeneralVolumeSliderValueChanged(float value)
        {
            soundService.SetVolume(value);
        }

        private void OnGeneralVolumeSliderDragEnd()
        {
            gameSettingsRepository.GetSettingsSynchronously().generalVolume = soundService.GetVolume();

            gameSettingsRepository.SaveSettingsSynchronously();
        }

        private void OnMusicSliderValueChanged(float value)
        {
            soundService.SetVolumeOfChannel(SoundChannels.Music, value);
        }

        private void OnMusicSliderDragEnd()
        {
            gameSettingsRepository.GetSettingsSynchronously().generalVolume = soundService.GetVolumeOfChannel(SoundChannels.Music);

            gameSettingsRepository.SaveSettingsSynchronously();
        }

        private void OnEffectsSliderValueChanged(float value)
        {
            soundService.SetVolumeOfChannel(SoundChannels.Effects, value);
        }

        private void OnEffectsSliderDragEnd()
        {
            gameSettingsRepository.GetSettingsSynchronously().generalVolume = soundService.GetVolumeOfChannel(SoundChannels.Effects);

            gameSettingsRepository.SaveSettingsSynchronously();
        }
    }
}


