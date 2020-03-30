using System.IO;
using UnityEditor;
using UnityEngine;

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

        [MenuItem("Build/AssetBundles/For Current Target")]
        public static void ForCurrentBuildTarget()
        {
            switch(EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    ForWindows();
                    break;
                case BuildTarget.StandaloneOSX:
                    ForMacOS();
                    break;
                default:
                    Debug.LogWarning("NO SUITABLE ASSET BUNDLE BUILD TARGET FOUND");
                    break;
            }
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


