﻿using SJ.Localization;
using SJ.Updatables;
using SJ.Audio;
using SJ.Coroutines;
using UnityEngine.SceneManagement;
using UniRx;
using SJ.Management;
using UnityEngine;
using System;

namespace SJ
{
    public static class Application
    {
        public static IUpdater Updater { get; private set; }
        public static ITranslatorService TranslatorService { get; private set; }
        public static ISoundService SoundService { get; private set; }
        public static ICoroutineScheduler CoroutineScheduler { get; private set; }
        public static IGameManager GameManager { get; private set; }
        public static ApplicationSettings ApplicationSettings { get; private set; }

        public static event Action OnInitialized;

        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            ApplicationSettings = LoadApplicationSettings();

            Updater = UpdaterFactory.Create();
            TranslatorService = TranslatorServiceFactory.Create();
            SoundService = SoundServiceFactory.Create();
            CoroutineScheduler = CoroutineSchedulerFactory.Create();
            GameManager = new GameManager(Repositories.GetProfileRepository(), ApplicationSettings);

            Repositories.GetGameSettingsRepository().GetSettings()
                .ObserveOnMainThread()
                .Subscribe(gameSettings =>
                {
                    TranslatorService.ChangeLanguage(gameSettings.userLanguage);

                    SoundService.SetVolume(gameSettings.generalVolume);
                    SoundService.SetVolumeOfChannel(SoundChannels.Music, gameSettings.musicVolume);
                    SoundService.SetVolumeOfChannel(SoundChannels.Effects, gameSettings.effectsVolume);

                    OnInitialized?.Invoke();

                    IsInitialized = true;

                    SceneManager.LoadScene("Menu");
                });
        }

        private static ApplicationSettings LoadApplicationSettings()
        {
            return SJResources.LoadAsset<ApplicationSettings>(Reg.ApplicationSettingsAssetName);
        }

    }
}


