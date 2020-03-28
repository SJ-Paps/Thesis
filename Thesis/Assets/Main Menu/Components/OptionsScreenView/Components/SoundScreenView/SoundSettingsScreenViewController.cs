using SJ.Management.Audio;
using SJ.Management;
using System;
using UniRx;

namespace SJ.Menu
{
    public class SoundSettingsScreenViewController
    {
        private ISoundSettingsScreenView view;
        private IGameSettingsRepository gameSettingsRepository;
        private ISoundService soundService;
        private IOptionsScreenView optionsScreenView;

        public SoundSettingsScreenViewController(ISoundSettingsScreenView view, IGameSettingsRepository gameSettingsRepository, ISoundService soundService, IOptionsScreenView optionsScreenView)
        {
            this.view = view;
            this.gameSettingsRepository = gameSettingsRepository;
            this.soundService = soundService;
            this.optionsScreenView = optionsScreenView;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            view.OnGeneralSoundValueChanged += soundService.SetVolume;
            view.OnGeneralSoundValueConfirmed += value => SaveSettings(gameSettings => gameSettings.generalVolume = value);

            view.OnMusicSoundValueChanged += value => soundService.SetVolumeOfChannel(SoundChannels.Music, value);
            view.OnMusicSoundValueConfirmed += value => SaveSettings(gameSettings => gameSettings.musicVolume = value);

            view.OnEffectsSoundValueChanged += value => soundService.SetVolumeOfChannel(SoundChannels.Effects, value);
            view.OnEffectsSoundValueConfirmed += value => SaveSettings(gameSettings => gameSettings.effectsVolume = value);

            view.OnBackButtonClicked += optionsScreenView.FocusMainScreen;

            view.SetGeneralSoundValue(soundService.GetVolume());
            view.SetMusicSoundValue(soundService.GetVolumeOfChannel(SoundChannels.Music));
            view.SetEffectsSoundValue(soundService.GetVolumeOfChannel(SoundChannels.Effects));
        }

        private void SaveSettings(Action<GameSettings> saveAction)
        {
            gameSettingsRepository.GetSettings()
                .Do(gameSettings => saveAction(gameSettings))
                .ContinueWith(_ => gameSettingsRepository.SaveSettings())
                .ObserveOnMainThread()
                .Subscribe();
        }
    }

}