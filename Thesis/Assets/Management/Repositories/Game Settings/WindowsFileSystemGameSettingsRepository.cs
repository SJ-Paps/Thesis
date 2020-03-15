using SJ.Save;
using System;
using System.IO;
using UniRx;

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

        public IObservable<GameSettings> GetSettings()
        {
            return Observable.Create<GameSettings>(observer =>
            {
                if (gameSettings == null)
                {
                    gameSettings = LoadSettings();
                }

                observer.OnNext(gameSettings);
                observer.OnCompleted();

                return Disposable.Empty;
            });
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
            return Application.ApplicationSettings.DefaultGameSettings;
        }

        public IObservable<Unit> SaveSettings()
        {
            return Observable.Create<Unit>(observer =>
            {
                InternalSaveSettings(gameSettings);

                observer.OnNext(Unit.Default);
                observer.OnCompleted();

                return Disposable.Empty;
            });
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
    }
}