using System;
using System.IO;
using UnityEngine;

public static class Reg
{
    public static readonly string AssetBundleMainFileName = "AssetBundles";
    private static readonly string AssetBundleDirectoryBase = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
    
    public static readonly string AssetBundleDirectoryBuildWindows = Path.Combine(AssetBundleDirectoryBase, "Windows", AssetBundleMainFileName);
    public static readonly string AssetBundleDirectoryBuildMacOS = Path.Combine(AssetBundleDirectoryBase, "MacOS", AssetBundleMainFileName);
    
    #if UNITY_STANDALONE_WIN
    public static readonly string AssetBundleDirectory = AssetBundleDirectoryBuildWindows;
    #elif UNITY_STANDALONE_OSX
    public static readonly string AssetBundleDirectory = AssetBundleDirectoryBuildMacOS;
    #endif
    
    
    
    public const string ApplicationSettingsAssetName = "ApplicationSettings";
    public const string LanguageSettingsAssetName = "LanguageSettings";

#if UNITY_EDITOR
    public static readonly string BuildPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Application.productName), Application.productName + ".exe");
    public static readonly string AssetBundleDataPathBuild = Path.GetFullPath(Path.Combine(BuildPath, "../AssetBundles"));
    
#endif

    public static readonly int playerLayer = LayerMask.NameToLayer("Player");
    public static readonly int hostileLayer = LayerMask.NameToLayer("Hostile");
    public static readonly int nonHostileLayer = LayerMask.NameToLayer("NonHostile");
    public static readonly int hostileDeadlyLayer = LayerMask.NameToLayer("HostileDeadly");
    public static readonly int generalDeadlyLayer = LayerMask.NameToLayer("GeneralDeadly");
	public static readonly int floorLayer = LayerMask.NameToLayer("Floor");
    public static readonly int activableObjectLayer = LayerMask.NameToLayer("ActivableObject");
    public static readonly int movableObjectLayer = LayerMask.NameToLayer("MovableObject");
    public static readonly int playerDetectionLayer = LayerMask.NameToLayer("PlayerDetection");
    public static readonly int itemLayer = LayerMask.NameToLayer("Item");
    public static readonly int hiddenLayer = LayerMask.NameToLayer("Hidden");

    public static readonly int walkableLayerMask = (1 << floorLayer) | (1 << movableObjectLayer);
    public static readonly int activableLayerMask = (1 << movableObjectLayer) | (1 << activableObjectLayer);

    
    
}