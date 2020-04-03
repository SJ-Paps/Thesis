using UnityEditor;

namespace SJ.Editor
{
    public class Build
    {
        private static void For(BuildTarget target, BuildOptions options = BuildOptions.None)
        {
            SJUtilEditor.UpdatePrefabNameOfSaveableGameEntities();
            BuildAssetBundlesFor(target);

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Reg.BuildPath, target, options);
        }

        [MenuItem("Build/Windows/Production")]
        public static void ForWindowsProduction()
        {
            For(BuildTarget.StandaloneWindows);
        }

        [MenuItem("Build/Windows/Development")]
        public static void ForWindowsDevelopment()
        {
            For(BuildTarget.StandaloneWindows, BuildOptions.Development);
        }
        
        [MenuItem("Build/MacOS/Production")]
        public static void ForMacOSProduction()
        {
            For(BuildTarget.StandaloneOSX);
        }

        [MenuItem("Build/MacOS/Development")]
        public static void ForMacOSDevelopment()
        {
            For(BuildTarget.StandaloneOSX, BuildOptions.Development);
        }

        private static void BuildAssetBundlesFor(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                    BuildAssetBundles.ForWindows();
                    return;
                case BuildTarget.StandaloneOSX:
                    BuildAssetBundles.ForMacOS();
                    return;
            }
        }

    }

}