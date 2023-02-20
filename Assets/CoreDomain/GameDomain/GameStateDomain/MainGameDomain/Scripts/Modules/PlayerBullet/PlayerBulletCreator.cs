using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Bullet;
using CoreDomain.Scripts.Utils.Pools;
using CoreDomain.Services;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet
{
    public class PlayerBulletCreator
    {
        private readonly PlayerBulletPool _playerBulletPool;

        public PlayerBulletCreator(PlayerBulletPool.Factory playerBulletPoolFactory)
        {
            _playerBulletPool = playerBulletPoolFactory.Create(new PoolData(15, 5));
            _playerBulletPool.InitPool();
        }

        public PlayerBulletView CreateBullet()
        {
            return _playerBulletPool.Spawn();
        }
        
        public void DestroyBullet(PlayerBulletView playerBulletView)
        {
            _playerBulletPool.Despawn(playerBulletView);
        }

        // private const string PlayerBulletAssetName = "PlayerBullet";
        // private const string PlayerBulletAssetBundlePath = "coredomain/gamedomain/gamestatedomain/maingamedomain/playerbullet";
        // private readonly IAssetBundleLoaderService _assetBundleLoaderService;
        // private readonly DiContainer _diContainer;
        //
        // public PlayerBulletCreator(IAssetBundleLoaderService assetBundleLoaderService, DiContainer diContainer)
        // {
        //     _assetBundleLoaderService = assetBundleLoaderService;
        //     _diContainer = diContainer;
        // }
        //
        // public PlayerBulletView CreateBullet()
        // {
        //     var playerBulletView = _assetBundleLoaderService.LoadGameObjectAssetFromBundle(PlayerBulletAssetBundlePath, PlayerBulletAssetName);
        //     return _diContainer.InstantiatePrefab(playerBulletView).GetComponent<PlayerBulletView>();
        // }
    }
}