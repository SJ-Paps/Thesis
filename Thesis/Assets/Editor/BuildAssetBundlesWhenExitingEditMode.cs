using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SJ.Editor
{
    [InitializeOnLoad]
    public static class BuildAssetBundlesWhenExitingEditMode
    {
        static BuildAssetBundlesWhenExitingEditMode()
        {
            EditorApplication.playModeStateChanged += BuildAssetBundlesIfExitingEditMode;
        }

        private static void BuildAssetBundlesIfExitingEditMode(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                BuildAssetBundles.ForCurrentTarget();
        }
    }
}