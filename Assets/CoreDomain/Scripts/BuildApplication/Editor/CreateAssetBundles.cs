using System.IO;
using UnityEditor;
using UnityEngine;

namespace CoreDomain.BuildApplication
{
    public class CreateAssetBundles
    {
        private const string StreamingAssetsDirectory = "Assets/StreamingAssets";

        [MenuItem("Assets/Build AssetBundles")]
        private static void BuildAllAssetBundles()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(StreamingAssetsDirectory);
            }

            BuildPipeline.BuildAssetBundles(StreamingAssetsDirectory, BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget);

            AssetDatabase.Refresh();
        }
    }
}