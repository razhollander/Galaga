﻿using Cysharp.Threading.Tasks;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies
{
    public interface IEnemiesModule
    {
        UniTaskVoid DoEnemiesWavesSequence(EnemiesWaveSequenceData[] enemiesWaveSequenceData);
        bool TryKillEnemy(string enemyHitId);
        int GetEnemyScore(string enemyId);
    }
}