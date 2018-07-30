using UnityEngine;
using System.IO;

public static class Reg
{

    public const string languageListFileName = "langl";
    public const string languageFileName = "lang";
    public const string languageListAssetName = "langList";
    public const string languageAssetName = "language";

#if UNITY_EDITOR
    public static readonly string assetBundleDataPath = Application.dataPath + "/AssetBundles";
#else
    public static readonly string assetBundleDataPath = Application.dataPath + @"\..\Resources";
#endif

    public static readonly string assetBundleManifestPath = assetBundleDataPath + "/AssetBundles";
}