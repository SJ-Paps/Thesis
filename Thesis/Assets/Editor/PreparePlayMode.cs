using UnityEditor;

namespace SJ.Editor
{
    [InitializeOnLoad]
    public static class PreparePlayMode
    {
        static PreparePlayMode()
        {
            EditorApplication.playModeStateChanged += PrepareIfExitingEditMode;
        }

        private static void PrepareIfExitingEditMode(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                Prepare();
        }

        private static void Prepare()
        {
            SJUtilEditor.UpdatePrefabNameOfSaveableGameEntities();
            BuildAssetBundles.ForCurrentBuildTarget();
        }
    }
}