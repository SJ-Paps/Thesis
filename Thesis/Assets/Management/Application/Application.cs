using SJ.Localization;
using SJ.Updatables;
using SJ.Audio;
using SJ.Coroutines;
using UnityEngine.SceneManagement;
using UniRx;

namespace SJ
{
    public static class Application
    {
        private static IUpdater updater;
        private static ITranslatorService translatorService;
        private static ISoundService soundService;
        private static ICoroutineScheduler coroutineScheduler;

        private static ApplicationSettings applicationSettings;

        public static void Initialize()
        {
            applicationSettings = LoadApplicationSettings();

            updater = UpdaterFactory.Create();
            translatorService = TranslatorServiceFactory.Create();
            soundService = SoundServiceFactory.Create();
            coroutineScheduler = CoroutineSchedulerFactory.Create();

            Repositories.GetGameSettingsRepository().GetSettings()
                .Subscribe(gameSettings =>
                {
                    translatorService.ChangeLanguage(gameSettings.userLanguage);

                    soundService.SetVolume(gameSettings.generalVolume);
                    soundService.SetVolumeOfChannel(SoundChannels.Music, gameSettings.musicVolume);
                    soundService.SetVolumeOfChannel(SoundChannels.Effects, gameSettings.soundsVolume);

                    SceneManager.LoadScene("Menu");
                });
        }

        private static ApplicationSettings LoadApplicationSettings()
        {
            return SJResources.LoadAsset<ApplicationSettingsAsset>(Reg.APPLICATION_INFO_ASSET_NAME).GetApplicationSettings();
        }

        public static ApplicationSettings GetApplicationSettings()
        {
            return applicationSettings;
        }

        public static IUpdater GetUpdater()
        {
            return updater;
        }

        public static ITranslatorService GetTranslatorService()
        {
            return translatorService;
        }

        public static ISoundService GetSoundService()
        {
            return soundService;
        }

        public static ICoroutineScheduler GetCoroutineScheduler()
        {
            return coroutineScheduler;
        }

    }
}


