using System.IO;
using UnityEngine;

public static class Reg
{
    public static readonly string AssetBundleMainFileName = "AssetBundles";
    private static readonly string AssetBundleDirectoryBase = Path.Combine(Application.streamingAssetsPath, "AssetBundles");

    public static readonly string ConfigurationFilesPath = Path.Combine(Application.persistentDataPath, "GameConfiguration");

    public static readonly string AssetBundleDirectoryBuildWindows = Path.Combine(AssetBundleDirectoryBase, "Windows", AssetBundleMainFileName);
    public static readonly string AssetBundleDirectoryBuildMacOS = Path.Combine(AssetBundleDirectoryBase, "MacOS", AssetBundleMainFileName);
    
    #if UNITY_STANDALONE_WIN
    public static readonly string AssetBundleDirectory = AssetBundleDirectoryBuildWindows;
    #elif UNITY_STANDALONE_OSX
    public static readonly string AssetBundleDirectory = AssetBundleDirectoryBuildMacOS;
    #endif
    
    public const string ApplicationSettingsAssetName = "ApplicationSettings";
    public const string LanguageSettingsAssetName = "LanguageSettings";
    public const string DefaultGameSettingsAssetName = "DefaultGameSettings";
    public const string DefaultGameInputSettingsAssetName = "DefaultGameInputSettings";

#if UNITY_EDITOR
    public static readonly string ProjectPath = Application.dataPath.Replace("/Assets", "");
    public static readonly string BuildFolderPath = Path.Combine(ProjectPath, "Builds");
    public static readonly string BuildPath = Path.Combine(Path.Combine(BuildFolderPath, Application.productName), Application.productName + ".exe");
#endif
}