using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using SJ.Save;

namespace SJ
{
    public class WindowsFileSystemGameSettingsRepository : IGameSettingsRepository
    {
        private static readonly string configurationFilePath = Path.Combine(UnityEngine.Application.dataPath, "../game_configuration/config.sj");

        private GameSettings gameSettings;

        private ISaveSerializer saveSerializer;

        public WindowsFileSystemGameSettingsRepository(ISaveSerializer saveSerializer)
        {
            this.saveSerializer = saveSerializer;
        }

        public Task<GameSettings> GetSettings()
        {
            return Task.Run(
                delegate()
                {
                    if(gameSettings == null)
                    {
                        gameSettings = LoadSettings();
                    }

                    return gameSettings;
                }
                );
        }

        private GameSettings LoadSettings()
        {
            if (File.Exists(configurationFilePath) == false)
            {
                InternalSaveSettings(GetDefault());
            }

            SaveData[] saves = saveSerializer.Deserialize(File.ReadAllText(configurationFilePath));

            if (saves == null || saves.Length == 0)
            {
                GameSettings gameSettings = GetDefault();

                InternalSaveSettings(gameSettings);

                return gameSettings;
            }
            else
            {
                SaveData configSave = saves[0];

                return (GameSettings)configSave.saveObject;
            }
        }

        private static GameSettings GetDefault()
        {
            return ApplicationInfo.DefaultGameConfiguration;
        }

        public Task SaveSettings()
        {
            return SaveSettings(gameSettings);
        }

        private void InternalSaveSettings(GameSettings settings)
        {
            string saveDirectory = Path.GetDirectoryName(configurationFilePath);

            if (Directory.Exists(saveDirectory) == false)
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string serialized = saveSerializer.Serialize(new SaveData("config", settings));

            File.WriteAllText(configurationFilePath, serialized);
        }

        public Task SaveSettings(GameSettings settings)
        {
            return Task.Run(() => InternalSaveSettings(settings));
        }
    }
}