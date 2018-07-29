using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles ChunkBased")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
    }
}
