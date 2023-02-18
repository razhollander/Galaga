using System.Collections.Generic;
using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesCreator
    {
        private const string MainGameUiAssetName = "PlayerBullet";
        private const string MainGameUiAssetBundlePath = "coredomain/gamedomain/gamestatedomain/maingamedomain/enemies";
        private readonly IAssetBundleLoaderService _assetBundleLoaderService;

        public EnemiesCreator(IAssetBundleLoaderService assetBundleLoaderService)
        {
            _assetBundleLoaderService = assetBundleLoaderService;
        }

        public EnemyView CreateEnemy(string enemyAssetName)
        {
            return _assetBundleLoaderService.InstantiateAssetFromBundle<EnemyView>(MainGameUiAssetBundlePath, enemyAssetName);
        }
        
        public EnemyView CreateEnemies(List<string> enemiesAssetNames)
        {
            var enemiesBundle = _assetBundleLoaderService.LoadAssetBundle(MainGameUiAssetBundlePath);
            _assetBundleLoaderService.lo
            return _assetBundleLoaderService.InstantiateAssetFromBundle<EnemyView>(MainGameUiAssetBundlePath, enemyAssetName);
        }
    }
}