using Paps.EventBus;
using SJ.Management.Audio;
using SJ.Management.Localization;
using System;
using UniRx;

namespace SJ.Management
{
    public static class Application
    {
        public static IUpdater Updater { get; private set; }
        public static ITranslatorService TranslatorService { get; private set; }
        public static ISoundService SoundService { get; private set; }
        public static IGameManager GameManager { get; private set; }
        public static IEventBus EventBus { get; private set; }
        public static ApplicationSettings ApplicationSettings { get; private set; }

        public static bool IsInitialized { get; private set; }

        public static void Initialize(LoadAction[] loadActions)
        {
            LoadApplicationSettings()
                .Do(applicationsSettings => ApplicationSettings = applicationsSettings)
                .Do(_ =>
                {
                    InitializeMandatoryObjects();
                    ExecuteLoadActions(loadActions);
                })
                .Subscribe();
        }

        private static void ExecuteLoadActions(LoadAction[] loadActions)
        {
            Observable.Zip(loadActions)
                .ObserveOnMainThread()
                .Subscribe();
        }

        private static void InitializeMandatoryObjects()
        {
            Updater = UpdaterFactory.Create();
            TranslatorService = TranslatorServiceFactory.Create();
            SoundService = SoundServiceFactory.Create();
            GameManager = new GameManager(Repositories.GetProfileRepository(), ApplicationSettings);
            EventBus = new EventBus();

            IsInitialized = true;
        }

        private static IObservable<ApplicationSettings> LoadApplicationSettings()
        {
            return SJResources.LoadAssetAsync<ApplicationSettings>(Reg.ApplicationSettingsAssetName);
        }

    }
}


