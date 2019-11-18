using SJ.Localization;
using SJ.Updatables;
using SJ.Audio;
using UnityEngine.SceneManagement;

namespace SJ
{
    public static class Application
    {
        private static IUpdater updater;
        private static ITranslatorService translatorService;
        private static ISoundService soundService;

        public static void Initialize()
        {
            updater = UpdaterFactory.Create();
            translatorService = TranslatorServiceFactory.Create();
            soundService = SoundServiceFactory.Create();

            GameConfiguration gameConfiguration = GameConfigurationCareTaker.GetConfiguration();

            translatorService.ChangeLanguage(gameConfiguration.userLanguage);

            soundService.SetVolume(gameConfiguration.generalVolume);
            soundService.SetVolumeOfChannel(SoundChannels.Music, gameConfiguration.musicVolume);
            soundService.SetVolumeOfChannel(SoundChannels.Effects, gameConfiguration.soundsVolume);

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

    }
}


