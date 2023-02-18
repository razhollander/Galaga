using System;
using System.Collections.Generic;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesModule : IEnemiesModule
    {
        private const string EnemyWaveParentName = "EnemiesWaveParent";
        private static readonly Vector2 RelativeToScreenCenterStartPosition = new(0.2f, 0.9f);

        private Transform _enemiesParentTransform;
        private readonly Vector2 _enemiesGroupStartPosition;
        private readonly EnemiesViewModule _enemiesViewModule;
        private readonly EnemiesCreator _enemiesCreator;
        private List<EnemyDataScriptableObject> _enemiesData = new List<EnemyDataScriptableObject>();
        
        public EnemiesModule(IDeviceScreenService deviceScreenService)
        {
            _enemiesViewModule = new EnemiesViewModule(deviceScreenService);
            _enemiesCreator = new EnemiesCreator();
        }

        public async UniTaskVoid DoEnemiesWavesSequence(EnemiesWaveSequenceData[] enemiesWaveSequenceData)
        {
            foreach (var enemiesWave in enemiesWaveSequenceData)
            {
                var enemiesView = CreateEnemiesForWave(enemiesWave);
                await _enemiesViewModule.DoEnemiesWaveSequence(enemiesWave);
            }
        }

        private void CreateEnemiesForWave(EnemiesWaveSequenceData enemiesWave)
        {
            for (int i = 0; i <  enemiesWave.EnemiesGrid.GetLength(0); i++)
            {
                for (int j = 0; j < enemiesWave.EnemiesGrid.GetLength(1); j++)
                {
                    var enemyView = _enemiesCreator.CreateEnemy(enemiesWave.EnemiesGrid[i, j].EnemyPathsData.Enemy);
                    var enemyId = Guid.NewGuid();
                    var enemyData = new EnemyData()
                    enemyView.Setup(enemyId);
                }                
            }
            
        }

        public void EnemyHit(EnemyView enemyViewHit)
        {
        }
    }
}