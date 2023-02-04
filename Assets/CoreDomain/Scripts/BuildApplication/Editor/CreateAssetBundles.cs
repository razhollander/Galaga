using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    #region --- Constants ---

    private const string streamingAssetsDirectory = "Assets/StreamingAssets";

    #endregion


    #region --- Private Methods ---

    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(streamingAssetsDirectory);
        }

        BuildPipeline.BuildAssetBundles(streamingAssetsDirectory, BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();
    }

    #endregion
}