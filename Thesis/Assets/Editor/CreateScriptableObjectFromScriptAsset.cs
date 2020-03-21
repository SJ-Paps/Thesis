using System.IO;
using UnityEditor;
using UnityEngine;

namespace SJ.Editor
{
    public static class CreateScriptableObjectFromScript
    {
        private const string MenuItemName = "Assets/Create/SJ/Scriptable Object From Selected Script";

        [MenuItem(MenuItemName)]
        public static void CreateAssetFromScript()
        {
            var script = Selection.activeObject as MonoScript;

            CreateAssetFromScript(script);
        }

        public static void CreateAssetFromScript(MonoScript script)
        {
            if(script != null && IsValidScriptType(script))
            {
                var scriptType = script.GetClass();

                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(scriptType), Path.ChangeExtension(AssetDatabase.GetAssetPath(script), ".asset"));
            }
        }

        [MenuItem(MenuItemName, validate = true)]
        private static bool IsValidScriptType()
        {
            var selectedObject = Selection.activeObject;

            if(selectedObject is MonoScript script)
            {
                return IsValidScriptType(script);
            }

            return false;
        }

        private static bool IsValidScriptType(MonoScript script)
        {
            var scriptClassType = script.GetClass();

            return scriptClassType.IsAbstract == false && typeof(ScriptableObject).IsAssignableFrom(scriptClassType);
        }
    }
}