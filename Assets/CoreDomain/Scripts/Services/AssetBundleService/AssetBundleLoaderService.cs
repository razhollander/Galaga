using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace CoreDomain.Services
{
    public class AssetBundleLoaderService : IAssetBundleLoaderService
    {
        public T InstantiateAssetFromBundle<T>(string bundlePathName, string assetName) where T : Object
        {
            return GameObject.Instantiate(LoadAssetFromBundle<T>(bundlePathName, assetName)).GetComponent<T>();
        }
        
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
    }
}