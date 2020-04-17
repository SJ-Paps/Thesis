using Paps.EventBus;
using SJ.Management;
using SJ.Management.Audio;
using SJ.Management.Localization;

public static class ApplicationExtensions
{
    private static IUpdater updater;
    private static ITranslatorService translatorService;
    private static ISoundService soundService;
    private static IGameManager gameManager;
    private static IEventBus eventBus;
    private static ApplicationSettings applicationSettings;

    public static IUpdater Updater(this Application application)
    {
        return updater ?? (updater = UpdaterFactory.Create());
    }

    public static ITranslatorService TranslatorService(this Application application)
    {
        return translatorService ?? (translatorService = TranslatorServiceFactory.Create());
    }

    public static ISoundService SoundService(this Application application)
    {
        return soundService ?? (soundService = SoundServiceFactory.Create());
    }

    public static IGameManager GameManager(this Application application)
    {
        return gameManager ?? (gameManager = GameManagerFactory.Create());
    }

    public static IEventBus EventBus(this Application application)
    {
        return eventBus ?? (eventBus = new EventBus());
    }

    public static ApplicationSettings ApplicationSettings(this Application application)
    {
        return applicationSettings ?? (applicationSettings = SJResources.LoadAsset<ApplicationSettings>(Reg.ApplicationSettingsAssetName));
    }
}
