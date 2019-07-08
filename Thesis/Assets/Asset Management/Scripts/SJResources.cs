using UnityEngine;

/// <summary>
/// Esta clase esta para reemplazar a la clase de Resources de Unity. Se puede acceder a los mismos recursos mediante AssetBundleLibrary, pidiendo el assetbundle llamado "resources"
/// </summary>
public static class SJResources
{
    private static AssetBundle resources;

    static SJResources()
    {
        resources = AssetBundleLibrary.Instance.GetAssetBundleByNameWithExtension(Reg.resourcesAssetBundleFileName);
    }

    public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object
    {
        T asset = resources.LoadAsset<T>(assetName);

        return asset;
    }
}
