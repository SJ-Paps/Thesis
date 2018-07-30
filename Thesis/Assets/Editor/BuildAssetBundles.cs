using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles ChunkBased")]
    static void BuildAllAssetBundles()
    {
        if(Directory.Exists(Reg.assetBundleDataPath) == false)
        {
            Directory.CreateDirectory(Reg.assetBundleDataPath);
        }

        BuildPipeline.BuildAssetBundles(Reg.assetBundleDataPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
    }
}
