using System.Collections.Generic;
using CoreDomain.Scripts.Utils.Pools;
using CoreDomain.Services;
using UnityEngine;
using Zenject;
using IPoolable = CoreDomain.Scripts.Utils.Pools.IPoolable;

public abstract class AssetFromBundlePool<T, TV> : BasePool<T, TV> where T : MonoBehaviour, IPoolable
{
    private DiContainer _diContainer;
    private IAssetBundleLoaderService _assetBundleLoaderService;
    protected abstract string AssetBundlePathName { get; } 
    protected abstract string AssetName { get; }
    protected abstract string ParentGameObjectName { get; }
    private Transform _parentGameObject;

    [Inject]
    private void Inject(DiContainer diContainer, IAssetBundleLoaderService assetBundleLoaderService)
    {
        Debug.Log("AssetFromBundlePool Inject");
    }

    public AssetFromBundlePool(PoolData poolData, DiContainer diContainer, IAssetBundleLoaderService assetBundleLoaderService) : base(poolData)
    {
        Debug.Log("AssetFromBundlePool");
        _diContainer = diContainer;
        _assetBundleLoaderService = assetBundleLoaderService;
    }

    public override void InitPool()
    {
        _parentGameObject = new GameObject(ParentGameObjectName).transform;
        base.InitPool();
    }

    protected override List<T> CreatePoolableInstances(int instancesAmount)
    {
        var poolables = new List<T>();
        var bundle = _assetBundleLoaderService.LoadAssetBundle(AssetBundlePathName);

        for (int i = 0; i < instancesAmount; i++)
        {
            var poolable = _diContainer.InstantiatePrefab(_assetBundleLoaderService.LoadAssetFromBundle<GameObject>(bundle, AssetName));
            poolable.SetActive(false);
            poolable.transform.SetParent(_parentGameObject);
            poolables.Add(poolable.GetComponent<T>());
        }
        
        _assetBundleLoaderService.UnloadAssetBundle(bundle);

        return poolables;
    }
}
