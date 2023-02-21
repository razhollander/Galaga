using System.Collections.Generic;
using CoreDomain.Services;
using UnityEngine;
using Zenject;

namespace CoreDomain.Utils.Pools
{
    public abstract class AssetFromBundlePool<TPoolable, TV> : BasePool<TPoolable, TV>, IAssetFromBundlePool<TPoolable> where TPoolable : MonoBehaviour, IPoolable
    {
        private DiContainer _diContainer;
        private IAssetBundleLoaderService _assetBundleLoaderService;
        protected abstract string AssetBundlePathName { get; }
        public abstract string AssetName { get; }
        protected abstract string ParentGameObjectName { get; }
        private Transform _parentGameObject;

        public AssetFromBundlePool(PoolData poolData, DiContainer diContainer, IAssetBundleLoaderService assetBundleLoaderService) : base(poolData)
        {
            _diContainer = diContainer;
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public override void InitPool()
        {
            _parentGameObject = new GameObject(ParentGameObjectName).transform;
            base.InitPool();
        }

        protected override List<TPoolable> CreatePoolableInstances(int instancesAmount)
        {
            var poolables = new List<TPoolable>();
            var bundle = _assetBundleLoaderService.LoadAssetBundle(AssetBundlePathName);

            for (int i = 0; i < instancesAmount; i++)
            {
                var poolable = _diContainer.InstantiatePrefab(_assetBundleLoaderService.LoadAssetFromBundle<GameObject>(bundle, AssetName));
                poolable.SetActive(false);
                poolable.transform.SetParent(_parentGameObject);
                poolables.Add(poolable.GetComponent<TPoolable>());
            }

            _assetBundleLoaderService.UnloadAssetBundle(bundle);

            return poolables;
        }

        protected override void Despawn(TPoolable obj)
        {
            if (obj == null)
            {
                return;
            }
            
            obj.transform.SetParent(_parentGameObject);
            base.Despawn(obj);
        }
    }
}