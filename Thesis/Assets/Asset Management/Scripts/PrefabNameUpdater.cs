using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabNameUpdater : UnityEditor.AssetModificationProcessor
{
    private static string[] OnWillSaveAssets(string[] paths)
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        List<SJMonoBehaviour> allWantedPrefabs = new List<SJMonoBehaviour>();

        for(int i = 0; i < allAssets.Length; i++)
        {
            string current = allAssets[i];

            if(current.Contains(".prefab"))
            {
                SJMonoBehaviour prefabBehaviour = AssetDatabase.LoadAssetAtPath<GameObject>(current).GetComponent<SJMonoBehaviour>();

                if(prefabBehaviour != null)
                {
                    Debug.Log(prefabBehaviour.name);

                    prefabBehaviour.prefabName = prefabBehaviour.name;

                    Debug.Log(prefabBehaviour.prefabName);
                }
            }
        }

        return paths;
    }
}
