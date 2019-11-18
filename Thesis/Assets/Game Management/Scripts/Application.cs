using SJ.Localization;
using SJ.Updatables;
using SJ.Audio;
using SJ.Coroutines;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace SJ
{
    public static class Application
    {
        private static IUpdater updater;
        private static ITranslatorService translatorService;
        private static ISoundService soundService;
        private static ICoroutineScheduler coroutineScheduler;

        public static void Initialize()
        {
            ApplicationInfo.Load();

            updater = UpdaterFactory.Create();
            translatorService = TranslatorServiceFactory.Create();
            soundService = SoundServiceFactory.Create();
            coroutineScheduler = CoroutineSchedulerFactory.Create();

            GameSettings gameSettings = Repositories.GetGameSettingsRepository().GetSettingsSynchronously();

            translatorService.ChangeLanguage(gameSettings.userLanguage);

            soundService.SetVolume(gameSettings.generalVolume);
            soundService.SetVolumeOfChannel(SoundChannels.Music, gameSettings.musicVolume);
            soundService.SetVolumeOfChannel(SoundChannels.Effects, gameSettings.soundsVolume);

            SceneManager.LoadScene("Menu");
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


