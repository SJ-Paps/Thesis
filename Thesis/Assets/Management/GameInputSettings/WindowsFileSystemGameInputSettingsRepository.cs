using System;
using System.IO;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    public class WindowsFileSystemGameInputSettingsRepository : IGameInputSettingsRepository
    {
        private const string GameInputSettingsFile = "game_input_settings.sj";
        private static readonly string ConfigurationFilePath = Path.Combine(Reg.ConfigurationFilesPath, GameInputSettingsFile);

        private GameInputSettings defaultGameInputSettings;
        private GameInputSettings gameInputSettings;

        public WindowsFileSystemGameInputSettingsRepository()
        {
            defaultGameInputSettings = SJResources.LoadAsset<GameInputSettings>(Reg.DefaultGameInputSettingsAssetName);
        }

        public IObservable<GameInputSettings> GetSettings()
        {
            if (gameInputSettings != null)
                return Observable.Return(gameInputSettings);

            return Observable.Start(() =>
            {
                gameInputSettings = LoadSettings();
                return gameInputSettings;
            }, Scheduler.Immediate);
        }

        private GameInputSettings LoadSettings()
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
            SaveSettings(defaultGameInputSettings);
        }

        public IObservable<Unit> SaveSettings()
        {
            return GetSettings()
                .SelectMany(gameSettings => Observable.Start(() => SaveSettings(gameSettings), Scheduler.Immediate));
        }

        private void SaveSettings(GameInputSettings settings)
        {
            string saveDirectory = Path.GetDirectoryName(ConfigurationFilePath);

            if (Directory.Exists(saveDirectory) == false)
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string serialized = Serialize(settings);

            File.WriteAllText(ConfigurationFilePath, serialized);
        }

        private string Serialize(GameInputSettings gameSettings)
        {
            return JsonUtility.ToJson(gameSettings);
        }

        private GameInputSettings Deserialize(string json)
        {
            try
            {
                var gameInputSettings = ScriptableObject.CreateInstance<GameInputSettings>();
                JsonUtility.FromJsonOverwrite(json, gameInputSettings);
                return gameInputSettings;
            }
            catch
            {
                return null;
            }
        }
    }
}