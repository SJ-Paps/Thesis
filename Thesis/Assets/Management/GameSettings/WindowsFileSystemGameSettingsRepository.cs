using System;
using System.IO;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    public class WindowsFileSystemGameSettingsRepository : IGameSettingsRepository
    {
        private const string GameSettingsFile = "game_settings.sj";
        private static readonly string ConfigurationFilePath = Path.Combine(Reg.ConfigurationFilesPath, GameSettingsFile);

        private GameSettings defaultGameSettings;
        private GameSettings gameSettings;

        public WindowsFileSystemGameSettingsRepository()
        {
            defaultGameSettings = SJResources.LoadAsset<GameSettings>(Reg.DefaultGameSettingsAssetName);
        }

        public IObservable<GameSettings> GetSettings()
        {
            if (gameSettings != null)
                return Observable.Return(gameSettings);

            return Observable.Start(() =>
            {
                gameSettings = LoadSettings();
                return gameSettings;
            }, Scheduler.Immediate);
        }

        private GameSettings LoadSettings()
        {
            if (File.Exists(ConfigurationFilePath) == false)
            {
                SaveDefault();
                return Deserialize(File.ReadAllText(ConfigurationFilePath));
            }
            else
            {
                var settings = Deserialize(File.ReadAllText(ConfigurationFilePath));

                if (settings == null)
                {
                    SaveDefault();
                    return Deserialize(File.ReadAllText(ConfigurationFilePath));
                }
                else
                {
                    return settings;
                }
            }
        }

        private void SaveDefault()
        {
            SaveSettings(defaultGameSettings);
        }

        public IObservable<Unit> SaveSettings()
        {
            return GetSettings()
                .SelectMany(gameSettings => Observable.Start(() => SaveSettings(gameSettings)));
        }

        private void SaveSettings(GameSettings settings)
        {
            string saveDirectory = Path.GetDirectoryName(ConfigurationFilePath);

            if (Directory.Exists(saveDirectory) == false)
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string serialized = Serialize(settings);

            File.WriteAllText(ConfigurationFilePath, serialized);
        }

        private string Serialize(GameSettings gameSettings)
        {
            return JsonUtility.ToJson(gameSettings);
        }

        private GameSettings Deserialize(string json)
        {
            try
            {
                var gameSettings = ScriptableObject.CreateInstance<GameSettings>();
                JsonUtility.FromJsonOverwrite(json, gameSettings);
                return gameSettings;
            }
            catch
            {
                return null;
            }
        }
    }
}