using System;
using System.Collections.Generic;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Services.Audio;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public class EnemiesModule : IEnemiesModule
    {
        private const string LevelStartMusicName = "GalagaLevelStartSoundEffect";

        private readonly IAudioService _audioService;
        private Transform _enemiesParentTransform;
        private readonly EnemiesViewModule _enemiesViewModule;
        private readonly EnemiesCreator _enemiesCreator;
        private Dictionary<string, EnemyData> _enemiesData = new ();
        
        public EnemiesModule(IDeviceScreenService deviceScreenService, BeeEnemiesPool.Factory beeEnemiesPoolFactory, GuardEnemiesPool.Factory guardEnemiesPoolFactory, IAudioService audioService)
        {
            _audioService = audioService;
            _enemiesViewModule = new EnemiesViewModule(deviceScreenService, CreateEnemy);
            _enemiesCreator = new EnemiesCreator(beeEnemiesPoolFactory, guardEnemiesPoolFactory);
        }

        public int GetEnemyScore(string enemyId)
        {
            return _enemiesData[enemyId].Score;
        }
        
        public async UniTaskVoid DoEnemiesWavesSequence(EnemiesWaveSequenceData[] enemiesWaveSequenceData)
        {
            foreach (var waveSequenceData in enemiesWaveSequenceData)
            {
                _audioService.PlayAudio(LevelStartMusicName, AudioChannelType.Music, AudioPlayType.OneShot);
                await _enemiesViewModule.DoEnemiesWaveSequence(waveSequenceData);

                KillAllEnemies();
            }
        }

        private void KillAllEnemies()
        {
            _enemiesData.ForEach(x => _enemiesViewModule.KillEnemy(x.Key));
            _enemiesData.Clear();
        }

        private EnemyView CreateEnemy(EnemyDataScriptableObject enemyScriptableObject)
        {
            var enemyId = Guid.NewGuid().ToString();
            var enemyData = new EnemyData(enemyId, enemyScriptableObject.Score);
            _enemiesData.Add(enemyId, enemyData);
            
            var enemyView = _enemiesCreator.CreateEnemy(enemyScriptableObject.EnemyAssetName);
            enemyView.Setup(enemyId);
            return enemyView;
        }

        public bool TryKillEnemy(string enemyHitId)
        {
            if (!_enemiesData.ContainsKey(enemyHitId))
            {
                return false;
            }

            _enemiesData.Remove(enemyHitId);
            _enemiesViewModule.KillEnemy(enemyHitId);
            return true;
        }
    }
}