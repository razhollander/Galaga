using UnityEngine;

namespace CoreDomain.Services
{
    public interface IAssetBundleLoaderService
    {
        bool A();
        bool TryInstantiateAssetFromBundle<T>(string bundlePathName, string assetName, out T asset) where T : Object;
        bool TryLoadGameObjectAssetFromBundle(string bundlePathName, string assetName, out GameObject asset);
        bool TryLoadScriptableObjectAssetFromBundle<T>(string bundlePathName, string assetName, out T asset) where T : ScriptableObject;
        T InstantiateAssetFromBundle<T>(string bundlePathName, string assetName) where T : Object;
        GameObject LoadGameObjectAssetFromBundle(string bundlePathName, string assetName);
        T LoadScriptableObjectAssetFromBundle<T>(string bundlePathName, string assetName) where T : ScriptableObject;
    }
}