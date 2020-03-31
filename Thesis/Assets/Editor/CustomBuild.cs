using UnityEditor;

namespace SJ.Editor
{
    public class CustomBuild
    {
        private static void For(BuildOptions options, BuildTarget target)
        {
            BuildAssetBundlesFor(target);

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Reg.BuildPath, target, options);
        }

        [MenuItem("Build/Windows/Production")]
        public static void WindowsProduction()
        {
            For(BuildOptions.None, BuildTarget.StandaloneWindows);
        }

        [MenuItem("Build/Windows/Development")]
        public static void WindowsDevelopment()
        {
            For(BuildOptions.Development, BuildTarget.StandaloneWindows);
        }
        
        [MenuItem("Build/MacOS/Production")]
        public static void MacOSProduction()
        {
            For(BuildOptions.None, BuildTarget.StandaloneOSX);
        }

        [MenuItem("Build/MacOS/Development")]
        public static void MacOSDevelopment()
        {
            For(BuildOptions.Development, BuildTarget.StandaloneOSX);
        }

        private static void BuildAssetBundlesFor(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                    Editor.BuildAssetBundles.ForWindows();
                    return;
                case BuildTarget.StandaloneOSX:
                    Editor.BuildAssetBundles.ForMacOS();
                    return;
            }
        }

    }

}