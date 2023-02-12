using UnityEngine;

namespace Systems
{
    public interface IAssetBundleSystem
    {
        T LoadAssetFromBundle<T>(string bundlePathName, string assetName) where T : Object;
        T LoadAssetFromBundle<T>(AssetBundle assetbundle, string assetName) where T : Object;
        AssetBundle LoadBundle(string bundlePathName);
        void UnloadAssetBundle(AssetBundle assetBundle);
    }
}