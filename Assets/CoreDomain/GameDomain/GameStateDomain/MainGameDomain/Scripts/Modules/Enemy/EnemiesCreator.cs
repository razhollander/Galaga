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
        
        public EnemyView[,] CreateEnemiesWave(string[,] enemiesAssetNames)
        {
            var enemiesBundle = _assetBundleLoaderService.LoadAssetBundle(MainGameUiAssetBundlePath);
            var enemiesRows = enemiesAssetNames.GetLength(0);
            var enemiesColumns = enemiesAssetNames.GetLength(1);
            var enemyViews = new EnemyView[enemiesRows,enemiesColumns];

            for (int i = 0; i < enemiesRows; i++)
            {
                for (int j = 0; j < enemiesColumns; j++)
                {
                    enemyViews[i,j] = GameObject.Instantiate(_assetBundleLoaderService.LoadAssetFromBundle<GameObject>(enemiesBundle, enemiesAssetNames[i,j])).GetComponent<EnemyView>();
                }                
            }

            _assetBundleLoaderService.UnloadAssetBundle(enemiesBundle);
            
            return enemyViews;
        }
    }
}