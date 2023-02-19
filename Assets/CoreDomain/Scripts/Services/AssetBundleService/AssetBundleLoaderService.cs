using System.IO;
using UnityEngine;

namespace CoreDomain.Services
{
    public class AssetBundleLoaderService : IAssetBundleLoaderService
    {
        public T InstantiateAssetFromBundle<T>(string bundlePathName, string assetName) where T : Object
        {
            return GameObject.Instantiate(LoadGameObjectAssetFromBundle(bundlePathName, assetName)).GetComponent<T>();
        }
        
        public T InstantiateAssetFromBundle<T>(AssetBundle assetBundle, string assetName) where T : Object
        {
            return GameObject.Instantiate(LoadGameObjectAssetFromBundle(assetBundle, assetName)).GetComponent<T>();
        }

        public GameObject LoadGameObjectAssetFromBundle(string bundlePathName, string assetName)
        {
            var assetBundle = LoadAssetBundle(bundlePathName);

            return LoadGameObjectAssetFromBundle(assetBundle, assetName);
        }
        
        public GameObject LoadGameObjectAssetFromBundle(AssetBundle assetBundle, string assetName)
        {
            var asset = LoadAssetFromBundle<GameObject>(assetBundle, assetName);
            
            UnloadAssetBundle(assetBundle);
            return asset;
        }

        public T LoadScriptableObjectAssetFromBundle<T>(string bundlePathName, string assetName) where T : ScriptableObject
        {
            var assetBundle = LoadAssetBundle(bundlePathName);
            var asset = LoadAssetFromBundle<T>(assetBundle, assetName);

            UnloadAssetBundle(assetBundle);
            return asset;
        }

        public bool TryInstantiateAssetFromBundle<T>(string bundlePathName, string assetName, out T asset) where T : Object
        {
            if (TryLoadGameObjectAssetFromBundle(bundlePathName, assetName, out var gameObject))
            {
                asset = GameObject.Instantiate(gameObject).GetComponent<T>();
                return true;
            }

            asset = null;
            return false;
        }

        public bool TryLoadGameObjectAssetFromBundle(string bundlePathName, string assetName, out GameObject asset)
        {
            if (TryLoadAssetBundle(bundlePathName, out var assetBundle) && TryLoadAssetFromBundle(assetBundle, assetName, out asset))
            {              
                UnloadAssetBundle(assetBundle);
                return true;
            }   
           
            asset = null;
            return false;
        }

        public bool TryLoadScriptableObjectAssetFromBundle<T>(string bundlePathName, string assetName, out T asset) where T : ScriptableObject
        {
            if (TryLoadAssetBundle(bundlePathName, out var assetBundle) && TryLoadAssetFromBundle(assetBundle, assetName, out asset))
            {
                UnloadAssetBundle(assetBundle);
                return true;
            }

            asset = null;
            return false;
        }

        private bool TryLoadAssetBundle(string assetBundlePathName, out AssetBundle assetBundle)
        {
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundlePathName));

            if (assetBundle == null)
            {
                LogService.LogWarning("Failed to load AssetBundle at path " + assetBundlePathName);
                return false;
            }

            return true;
        }

        private bool TryLoadAssetFromBundle<T>(AssetBundle assetbundle, string assetName, out T asset) where T : Object
        {
            asset = assetbundle.LoadAsset<T>(assetName);

            if (asset == null)
            {
                LogService.LogWarning("Failed to load Asset " + assetName);
                return false;
            }

            return true;
        }

        public AssetBundle LoadAssetBundle(string assetBundlePathName)
        {
            var assetBundle =  AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundlePathName));

            if (assetBundle == null)
            {
                LogService.LogWarning("Failed to load AssetBundle at path " + assetBundlePathName);
            }

            return assetBundle;
        }

        public T LoadAssetFromBundle<T>(AssetBundle assetbundle, string assetName) where T : Object
        {
            var asset = assetbundle.LoadAsset<T>(assetName);

            if (asset == null)
            {
                LogService.LogWarning("Failed to load Asset " + assetName);
            }

            return asset;
        }

        public void UnloadAssetBundle(AssetBundle assetBundle)
        {
            assetBundle.Unload(false);
        }
    }
}