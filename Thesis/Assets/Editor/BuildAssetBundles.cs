using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Build/Build AssetBundles ChunkBased")]
    static void BuildAllAssetBundles()
    {
        BuildAllAssetBundles(Reg.assetBundleDataPath);
    }

    public static void BuildAllAssetBundles(string path)
    {
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
    }
}
