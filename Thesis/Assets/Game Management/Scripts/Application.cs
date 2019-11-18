using SJ.Localization;
using SJ.Updatables;
using UnityEngine.SceneManagement;

namespace SJ
{
    public static class Application
    {
        private static IUpdater updater;
        private static ITranslatorService translatorService;

        public static void Initialize()
        {
            updater = UpdaterFactory.Create();
            translatorService = TranslatorServiceFactory.Create();

            translatorService.ChangeLanguage(GameConfigurationCareTaker.GetConfiguration().userLanguage);

            GameConfiguration gameConfiguration = GameConfigurationCareTaker.GetConfiguration();

            SoundManager.GetInstance().ChangeVolume(gameConfiguration.generalVolume);
            SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Music].ChangeVolume(gameConfiguration.musicVolume);
            SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Effects].ChangeVolume(gameConfiguration.soundsVolume);

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

    }
}


