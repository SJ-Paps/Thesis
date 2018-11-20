using UnityEditor;

public class CustomBuild
{
    private static void Build(BuildOptions options)
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Reg.buildPath, BuildTarget.StandaloneWindows, options);

        CreateAssetBundles.BuildAllAssetBundles(Reg.assetBundleDataPathBuild);
    }
    
    private static void PlusVersion(float plusVersion)
    {
        float version = float.Parse(PlayerSettings.bundleVersion, System.Globalization.CultureInfo.InvariantCulture);
        version += plusVersion;

        PlayerSettings.bundleVersion = version.ToString();
        
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

    [MenuItem("Build/AutoVersionBuild")]
    private static void AutoVersionBuild()
    {
        PlusVersion(0.001f);
        Build(BuildOptions.None);
    }

    [MenuItem("Build/AutoVersionDevelopment")]
    private static void AutoVersionDevelopmentBuild()
    {
        PlusVersion(0.001f);
        Build(BuildOptions.Development);
    }
    
}
