using System.IO;
using UnityEngine;

namespace Systems
{
    public class AssetBundleSystem
    {
        #region --- Public Methods ---

        public T LoadAssetFromBundle<T>(string bundlePathName, string assetName) where T : Object
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundlePathName));

            if (assetBundle == null)
            {
                Debug.LogWarning("Failed to load AssetBundle at path " + bundlePathName);

                return null;
            }

            var asset = assetBundle.LoadAsset<T>(assetName);
            UnloadAssetBundle(assetBundle);

            return asset;
        }

        public T LoadAssetFromBundle<T>(AssetBundle assetbundle, string assetName) where T : Object
        {
            var asset = assetbundle.LoadAsset<T>(assetName);

            return asset;
        }

        public AssetBundle LoadBundle(string bundlePathName)
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundlePathName));

            if (assetBundle == null)
            {
                Debug.LogWarning("Failed to load AssetBundle at path " + bundlePathName);
            }

            return assetBundle;
        }

        public void UnloadAssetBundle(AssetBundle assetBundle)
        {
            assetBundle.Unload(false);
        }

        #endregion
    }
}