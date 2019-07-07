using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Paps.Unity;

public class AssetLibrary : SJMonoBehaviour
{
    private static AssetLibrary instance;

    public static AssetLibrary GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<AssetLibrary>();

            instance.Init();
        }

        return instance;
    }

    private Dictionary<string, Object> assets;

    [SerializeField]
    private Object[] serializedAssets;

    private bool isInitialized;

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    private void Init()
    {
        if(isInitialized == false)
        {
            assets = new Dictionary<string, Object>();

            for(int i = 0; i < serializedAssets.Length; i++)
            {
                assets.Add(serializedAssets[i].name, serializedAssets[i]);
            }

            UnityUtil.DontDestroyOnLoad(gameObject);

            isInitialized = true;
        }
    }

    public T GetAsset<T>(string name) where T : class
    {
        if(assets.ContainsKey(name) && assets[name] is T asset)
        {
            return asset;
        }

        return default;
    }
}
