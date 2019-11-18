using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SJ.Updatables;
using System;
using UnityEngine.SceneManagement;

namespace SJ
{
    public static class Application
    {
        private static IUpdater updater;

        public static void Initialize()
        {
            updater = UpdaterFactory.Create();

            LanguageInfo[] languageInfo = SJResources.LoadAsset<LanguageInfoAsset>(Reg.LANGUAGE_INFO_ASSET_NAME).LanguageInfo;

            LanguageManager.LoadLanguageInfo(languageInfo, GetDefaultLanguageIndex(languageInfo));

            GameConfiguration gameConfiguration = GameConfigurationCareTaker.GetConfiguration();

            SoundManager.GetInstance().ChangeVolume(gameConfiguration.generalVolume);
            SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Music].ChangeVolume(gameConfiguration.musicVolume);
            SoundManager.GetInstance().Channels[SoundManager.SoundChannels.Effects].ChangeVolume(gameConfiguration.soundsVolume);

            SceneManager.LoadScene("Menu");
        }

        private static int GetDefaultLanguageIndex(LanguageInfo[] languageInfo)
        {
            string defaultLanguage = GameConfigurationCareTaker.GetConfiguration().userLanguage;

            for (int i = 0; i < languageInfo.Length; i++)
            {
                if (languageInfo[i].Language == defaultLanguage)
                {
                    return i;
                }
            }

            throw new InvalidOperationException("default langauge mismatch");
        }

        public static IUpdater GetUpdater()
        {
            return updater;
        }

    }
}


