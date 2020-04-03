using UnityEditor;
using UnityEngine;
using System.Linq;
using SJ.GameEntities;
using Boo.Lang;

namespace SJ.Editor
{
    public static class SJUtilEditor
    {
        [MenuItem("GameObject/Create Organizational", priority = 0)]
        public static void CreateOrganizational()
        {
            GameObject go = new GameObject("-----------------------------");
            go.hideFlags = HideFlags.DontSaveInBuild;
        }

        [MenuItem("SJ Utils/Update Prefab Name of Saveable Game Entities")]
        public static void UpdatePrefabNameOfSaveableGameEntities()
        {
            var assetGuids = AssetDatabase.FindAssets("t: GameObject");

            var assetPaths = assetGuids.Select(guid => AssetDatabase.GUIDToAssetPath(guid));

            var saveableGameObjects = assetPaths.Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path))
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