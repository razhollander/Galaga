using System.Collections.Generic;
using CoreDomain.Scripts.Utils.Pools;
using CoreDomain.Services;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesCreator
    {
        private readonly BeeEnemiesPool _beeEnemiesPool;
        private readonly GuardEnemiesPool _guardEnemiesPool;
        private readonly List<IAssetFromBundlePool<EnemyView>> _enemiesPools = new();
        
        public EnemiesCreator(BeeEnemiesPool.Factory beeEnemiesPoolFactory, GuardEnemiesPool.Factory guardEnemiesPoolFactory)
        {
            _enemiesPools.Add(beeEnemiesPoolFactory.Create(new PoolData(10, 5)));
            _enemiesPools.Add(guardEnemiesPoolFactory.Create(new PoolData(20, 5)));

            foreach (var enemiesPool in _enemiesPools)
            {
                enemiesPool.InitPool();
            }
        }

        public EnemyView CreateEnemy(string enemyAssetName)
        {
            return _enemiesPools.Find(x => x.AssetName == enemyAssetName).Spawn();
        }
        
        public void DestroyBullet(PlayerBulletView playerBulletView)
        {
            _playerBulletPool.Despawn(playerBulletView);
        }
        
        public EnemyView[,] CreateEnemiesWave(string[,] enemiesAssetNames)
        {
            var enemiesRows = enemiesAssetNames.GetLength(0);
            var enemiesColumns = enemiesAssetNames.GetLength(1);
            var enemyViews = new EnemyView[enemiesRows,enemiesColumns];

            for (int i = 0; i < enemiesRows; i++)
            {
                for (int j = 0; j < enemiesColumns; j++)
                {
                    enemyViews[i,j] = CreateEnemy(enemiesAssetNames[i,j]);
                }                
            }

            return enemyViews;
        }
    }
}