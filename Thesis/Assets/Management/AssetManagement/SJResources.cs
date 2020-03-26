using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;

namespace SJ.Management
{
    public static class SJResources
    {
        private static Dictionary<AssetBundle, string[]> assetBundleAssetNames;

        static SJResources()
        {
            assetBundleAssetNames = new Dictionary<AssetBundle, string[]>();

            AssetBundle manifestBundle = AssetBundle.LoadFromFile(Path.Combine(Reg.AssetBundleDirectory, "AssetBundles"));

            AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            string[] assetBundleNames = manifest.GetAllAssetBundles();

            for (int i = 0; i < assetBundleNames.Length; i++)
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Reg.AssetBundleDirectory, assetBundleNames[i]));

                if (assetBundle != null)
                {
                    assetBundleAssetNames.Add(assetBundle,
                        Array.ConvertAll(assetBundle.GetAllAssetNames(),
                        fullname => Path.GetFileNameWithoutExtension(fullname).ToLower()));
                }
            }
        }

        public static T LoadComponentOfGameObject<T>(string assetName)
        {
            return LoadAsset<GameObject>(assetName).GetComponent<T>();
        }

        public static IObservable<T> LoadComponentOfGameObjectAsync<T>(string assetName)
        {
            return LoadAssetAsync<GameObject>(assetName)
                .Select(gameObject => gameObject.GetComponent<T>());
        }

        public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            foreach (var keyValue in assetBundleAssetNames)
            {
                if (keyValue.Value.Contains(assetName.ToLower()))
                    return keyValue.Key.LoadAsset<T>(assetName);
            }

            return null;
        }

        public static IObservable<T> LoadAssetAsync<T>(string assetName) where T : UnityEngine.Object
        {
            AssetBundle targetAssetBundle = null;

            foreach (var keyValue in assetBundleAssetNames)
            {
                if (keyValue.Value.Contains(assetName.ToLower()))
                    targetAssetBundle = keyValue.Key;
            }

            if (targetAssetBundle != null)
                return targetAssetBundle.LoadAssetAsync<T>(assetName)
                    .AsAsyncOperationObservable()
                    .Select(request => request.asset as T);
            else
                return Observable.Return<T>(null);
        }

        public static void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}