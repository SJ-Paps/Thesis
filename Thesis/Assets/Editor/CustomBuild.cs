using UnityEditor;

namespace SJ.Editor
{
    public class CustomBuild
    {
        private static void Build(BuildOptions options, BuildTarget target)
        {
            BuildAssetBundles(target);

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Reg.BuildPath, target, options);
        }

        [MenuItem("Build/Windows/Production")]
        private static void WindowsProduction()
        {
            Build(BuildOptions.None, BuildTarget.StandaloneWindows);
        }

        [MenuItem("Build/Windows/Development")]
        private static void WindowsDevelopment()
        {
            Build(BuildOptions.Development, BuildTarget.StandaloneWindows);
        }
        
        [MenuItem("Build/MacOS/Production")]
        private static void MacOSProduction()
        {
            Build(BuildOptions.None, BuildTarget.StandaloneOSX);
        }

        [MenuItem("Build/MacOS/Development")]
        private static void MacOSDevelopment()
        {
            Build(BuildOptions.Development, BuildTarget.StandaloneOSX);
        }

        private static void BuildAssetBundles(BuildTarget target)
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