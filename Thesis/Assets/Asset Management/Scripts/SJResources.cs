using UnityEngine;

/// <summary>
/// Esta clase esta para reemplazar a la clase de Resources de Unity. Se puede acceder a los mismos recursos mediante AssetBundleLibrary, pidiendo el assetbundle llamado "resources"
/// </summary>
public class SJResources
{
    private static SJResources instance;

    public static SJResources Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SJResources();
            }

            return instance;
        }
    }

    private AssetBundle resources;

    private SJResources()
    {
        resources = AssetBundleLibrary.Instance.GetAssetBundleByNameWithExtension(Reg.resourcesAssetBundleFileName);
    }

    public T LoadAsset<T>(string assetName) where T : UnityEngine.Object
    {
        T asset = resources.LoadAsset<T>(assetName);

        return asset;
    }

    public T LoadGameObjectAndGetComponent<T>(string gameObjectAssetName) where T : Component
    {
        GameObject gameObject = resources.LoadAsset<GameObject>(gameObjectAssetName);

        return gameObject.GetComponent<T>();
    }
}
