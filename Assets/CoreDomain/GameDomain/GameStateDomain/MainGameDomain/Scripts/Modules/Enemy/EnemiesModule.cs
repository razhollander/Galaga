using System;
using System.Collections.Generic;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesModule : IEnemiesModule
    {
        private Transform _enemiesParentTransform;
        private readonly EnemiesViewModule _enemiesViewModule;
        private readonly EnemiesCreator _enemiesCreator;
        private Dictionary<string, EnemyData> _enemiesData = new ();
        
        public EnemiesModule(IDeviceScreenService deviceScreenService, IAssetBundleLoaderService assetBundleLoaderService)
        {
            _enemiesViewModule = new EnemiesViewModule(deviceScreenService);
            _enemiesCreator = new EnemiesCreator(assetBundleLoaderService);
        }

        public int GetEnemyScore(string enemyId)
        {
            return _enemiesData[enemyId].Score;
        }
        
        public async UniTaskVoid DoEnemiesWavesSequence(EnemiesWaveSequenceData[] enemiesWaveSequenceData)
        {
            foreach (var waveSequenceData in enemiesWaveSequenceData)
            {
                var enemiesView = CreateEnemies(waveSequenceData);
                await _enemiesViewModule.DoEnemiesWaveSequence(enemiesView, waveSequenceData);
            }
        }

        private EnemyView[,] CreateEnemies(EnemiesWaveSequenceData enemiesWave)
        {
            var enemyViews = CreateEnemiesViews(enemiesWave);
            CreateEnemiesData(enemiesWave.EnemiesGrid, enemyViews);
            return enemyViews;
        }

        private void CreateEnemiesData(EnemySequenceData[,] waveEnemiesGrid, EnemyView[,] enemyViews)
        {
            var enemiesRows = waveEnemiesGrid.GetLength(0);
            var enemiesColumns = waveEnemiesGrid.GetLength(1);

            for (int i = 0; i < enemiesRows; i++)
            {
                for (int j = 0; j < enemiesColumns; j++)
                {
                    CreateEnemyData(waveEnemiesGrid[i, j], enemyViews[i, j]);
                }
            }
        }

        private void CreateEnemyData(EnemySequenceData enemiesWave, EnemyView enemyView)
        {
            var enemyId = Guid.NewGuid().ToString();
            var enemyData = new EnemyData(enemyId, enemiesWave.EnemyPathsData.Enemy.Score);
            _enemiesData.Add(enemyId, enemyData);
            enemyView.Setup(enemyId);
        }

        private EnemyView[,] CreateEnemiesViews(EnemiesWaveSequenceData enemiesWave)
        {
            var enemiesAssetNames = GetEnemiesAssetNamesFromSequence(enemiesWave);
            var enemyViews = _enemiesCreator.CreateEnemiesWave(enemiesAssetNames);

            return enemyViews;
        }

        private static string[,] GetEnemiesAssetNamesFromSequence(EnemiesWaveSequenceData enemiesWave)
        {
            var enemiesRows = enemiesWave.EnemiesGrid.GetLength(0);
            var enemiesColumns = enemiesWave.EnemiesGrid.GetLength(1);
            var enemiesAssetNames = new string [enemiesRows, enemiesColumns];

            for (int i = 0; i < enemiesRows; i++)
            {
                for (int j = 0; j < enemiesColumns; j++)
                {
                    enemiesAssetNames[i, j] = enemiesWave.EnemiesGrid[i, j].EnemyPathsData.Enemy.EnemyAssetName;
                }
            }

            return enemiesAssetNames;
        }

        public void KillEnemy(string enemyHitId)
        {
            if (!_enemiesData.ContainsKey(enemyHitId))
            {
                return;
            }

            _enemiesData.Remove(enemyHitId);
            _enemiesViewModule.KillEnemy(enemyHitId);
        }
    }
}