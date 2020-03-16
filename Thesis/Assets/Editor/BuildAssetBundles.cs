using System.IO;
using UnityEditor;

namespace SJ.Editor
{
    public static class BuildAssetBundles
    {
        [MenuItem("Build/AssetBundles/For Windows")]
        public static void ForWindows()
        {
            For(Reg.AssetBundleDirectoryBuildWindows, BuildTarget.StandaloneWindows);
        }
        
        [MenuItem("Build/AssetBundles/For MacOS")]
        public static void ForMacOS()
        {
            For(Reg.AssetBundleDirectoryBuildMacOS, BuildTarget.StandaloneOSX);
        }
        
        private static void For(string path, BuildTarget target)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ChunkBasedCompression, target);
            
            AssetDatabase.Refresh();
        }
    }
}


