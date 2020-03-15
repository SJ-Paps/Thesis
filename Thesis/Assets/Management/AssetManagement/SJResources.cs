using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class SJResources
{
    private static List<AssetBundle> assetBundles;

    static SJResources()
    {
        assetBundles = new List<AssetBundle>();

        AssetBundle manifestBundle = AssetBundle.LoadFromFile(Path.Combine(Reg.AssetBundleDirectory, "AssetBundles"));

        AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] assetBundleNames = manifest.GetAllAssetBundles();

        for (int i = 0; i < assetBundleNames.Length; i++)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Reg.AssetBundleDirectory, assetBundleNames[i]));

            if (assetBundle != null)
            {
                assetBundles.Add(assetBundle);
            }
        }
    }

    public static T LoadAsset<T>(string assetName) where T : Object
    {
        for (int i = 0; i < assetBundles.Count; i++)
        {
            T asset = assetBundles[i].LoadAsset<T>(assetName);
            
            if (asset != null)
            {
                return asset;
            }
        }

        return null;
    }

    public static void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }
}
