using UnityEditor;
using UnityEngine;
using System.Linq;
using SJ.GameEntities;
using System.Collections.Generic;

namespace SJ.Editor
{
    public static class SJUtilEditor
    {
        [MenuItem("GameObject/SJ/Create Organizational", priority = 0)]
        public static void CreateOrganizational()
        {
            GameObject go = new GameObject("-----------------------------");
            go.hideFlags = HideFlags.DontSaveInBuild;
        }

        [MenuItem("SJ Utils/Update Prefab Name of Saveable Game Entities")]
        public static void UpdatePrefabNameOfSaveableGameEntities()
        {
            var saveableGameObjects = AssetDatabase.FindAssets("t: GameObject")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path))
                .Where(gameObject => gameObject.ParentOrChildContainsComponent<SaveableGameEntity>())
                .ToList();

            var saveableEntities = new List<SaveableGameEntity>();

            saveableGameObjects.ForEach(gameObject => saveableEntities.AddRange(gameObject.GetComponentsInChildren<SaveableGameEntity>()));

            foreach (var entity in saveableEntities)
                entity.SavePrefabName();

            AssetDatabase.SaveAssets();
        }
    }

}