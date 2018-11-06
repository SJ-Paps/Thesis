using UnityEngine;
using System.IO;
using System;

public static class Reg
{
    public const string languageListFileName = "langl";
    public const string languageFileName = "lang";
    public const string languageListAssetName = "langList";
    public const string languageAssetName = "language";

    public const string assetBundleManifestAssetName = "AssetBundles";

    public const string resourcesAssetBundleFileName = "resources";

#if UNITY_EDITOR
    public static readonly string assetBundleDataPath = Application.dataPath + "/AssetBundles";
    public static readonly string buildPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.productName), Application.productName + ".exe");
    public static readonly string assetBundleDataPathBuild = Path.GetFullPath(Path.Combine(buildPath, "../AssetBundles"));

#else
    public static readonly string assetBundleDataPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../AssetBundles"));

#endif

    public const int playerLayer = 8;
    public const int hostileLayer = 9;
    public const int nonHostileLayer = 10;
    public const int hostileDeadlyLayer = 11;
    public const int generalDeadlyLayer = 12;
	public const int floorLayer = 13;
    public const int hideLayer = 14;
    public const int objectLayer = 15;
    public const int onlyPlayerStuffLayer = 16;
    public const int wallLayer = 17;
    public const int itemLayer = 18;
    public const int ledgeLayer = 19;
}