using UnityEngine;
using SJ.Audio;
using UniRx;

namespace SJ.Menu
{
    public class SoundMenu : SJMonoBehaviour
    {
        [SerializeField]
        private SoundSlider general, music, effects;

        private ISoundService soundService;
        private IGameSettingsRepository gameSettingsRepository;

        protected override void SJAwake()
        {
            soundService = Application.SoundService;
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
            gameSettingsRepository.GetSettings()
                .Subscribe(gameSettings =>
                {
                    gameSettings.generalVolume = soundService.GetVolume();

                    gameSettingsRepository.SaveSettings().Subscribe();
                });
        }

        private void OnMusicSliderValueChanged(float value)
        {
            soundService.SetVolumeOfChannel(SoundChannels.Music, value);
        }

        private void OnMusicSliderDragEnd()
        {
            gameSettingsRepository.GetSettings()
                .Subscribe(gameSettings =>
                {
                    gameSettings.musicVolume = soundService.GetVolumeOfChannel(SoundChannels.Music);

                    gameSettingsRepository.SaveSettings().Subscribe();
                });
        }

        private void OnEffectsSliderValueChanged(float value)
        {
            soundService.SetVolumeOfChannel(SoundChannels.Effects, value);
        }

        private void OnEffectsSliderDragEnd()
        {
            gameSettingsRepository.GetSettings()
                .Subscribe(gameSettings =>
                {
                    gameSettings.soundsVolume = soundService.GetVolumeOfChannel(SoundChannels.Effects);

                    gameSettingsRepository.SaveSettings().Subscribe();
                });
        }
    }
}


