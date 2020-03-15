using UnityEditor;
using System.IO;

namespace SJ.Editor
{
    public class CustomBuild
    {
        private static void Build(BuildOptions options)
        {
            BuildAssetBundles();

            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Reg.BuildPath, BuildTarget.StandaloneWindows, options);
        }

        [MenuItem("Build/Build AssetBundles")]
        public static void BuildAssetBundles()
        {
            BuildAssetBundles(Reg.AssetBundleDirectory, BuildTarget.StandaloneWindows);
        }

        private static void BuildAssetBundles(string path, BuildTarget target)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, target);
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

}