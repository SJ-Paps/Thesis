using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class GameConfigurationCareTaker
{
    private static string configurationFilePath = Path.Combine(Application.dataPath, "../game_configuration/config.sj");

    private static GameConfiguration configuration;

    private static bool isInitialized;

    public static ref GameConfiguration GetConfiguration()
    {
        if(isInitialized == false)
        {
            LoadConfiguration();
        }

        return ref configuration;
    }

    public static Task LoadConfigurationAsync()
    {
        return Task.Run(LoadConfiguration);
    }

    public static void LoadConfiguration()
    {
        if(File.Exists(configurationFilePath) == false)
        {
            SetDefault();

            SaveConfiguration();
        }
        else
        {
            SaveData[] saves = SaveLoadTool.Deserialize(configurationFilePath);

            if(saves == null || saves.Length == 0)
            {
                SetDefault();

                SaveConfiguration();
            }
            else
            {
                SaveData configSave = saves[0];

                configuration = (GameConfiguration)configSave.saveObject;
            }
        }

        isInitialized = true;
    }

    public static void SetDefault()
    {
        configuration = GetDefault();

        SaveConfiguration();
    }

    public static Task SaveConfigurationAsync()
    {
        return Task.Run(SaveConfiguration);
    }

    public static void SaveConfiguration()
    {
        string saveDirectory = Path.GetDirectoryName(configurationFilePath);

        if (Directory.Exists(saveDirectory) == false)
        {
            Directory.CreateDirectory(saveDirectory);
        }

        SaveLoadTool.Serialize(configurationFilePath, new SaveData("config", configuration));
    }

    private static GameConfiguration GetDefault()
    {
        return ApplicationInfo.DefaultGameConfiguration;
    }
}
