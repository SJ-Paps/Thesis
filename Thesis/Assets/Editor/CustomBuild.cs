using UnityEditor;

public class CustomBuild
{
    private static void Build(BuildOptions options)
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Reg.buildPath, BuildTarget.StandaloneWindows, options);

        CreateAssetBundles.BuildAllAssetBundles(Reg.assetBundleDataPathBuild);
    }

    private static void PlusVersion()
    {

    }

    [MenuItem("Build/SimpleCustomBuild")]
    private static void SimpleCustomBuild()
    {
        Build(BuildOptions.None);
    }

    [MenuItem("Build/SimpleCustomDevelopmentBuild")]
    private static void SimpleCustomDevelopmentBuild()
    {
        Build(BuildOptions.Development);
    }

    private static void AutoVersionBuild()
    {

    }

    private static void AutoVersionDevelopmentBuild()
    {

    }
    
}
