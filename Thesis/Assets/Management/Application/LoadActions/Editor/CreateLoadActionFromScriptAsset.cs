using SJ.Management;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace SJ.Editor
{
    public class CreateLoadActionFromScriptAsset
    {
        [MenuItem("Assets/Create/Load Actions/From Selected Script")]
        public static void CreateLoadActionFromScript()
        {
            if(IsValidLoadActionScript())
            {
                var script = Selection.activeObject as MonoScript;

                var loadActionType = script.GetClass();

                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(loadActionType), Path.ChangeExtension(AssetDatabase.GetAssetPath(script), ".asset"));
            }
        }

        [MenuItem("Assets/Create/Load Actions/From Selected Script", validate = true)]
        public static bool IsValidLoadActionScript()
        {
            var selectedObject = Selection.activeObject;

            return selectedObject is MonoScript script && typeof(LoadAction).IsAssignableFrom(script.GetClass());
        }
    }
}