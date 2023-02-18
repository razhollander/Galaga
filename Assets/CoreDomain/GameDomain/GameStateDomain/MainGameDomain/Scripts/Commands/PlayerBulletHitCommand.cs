using System.Collections;
using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Score;
using CoreDomain.Scripts.Utils.Command;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands
{
    public class PlayerBulletHitCommand : CommandOneParameter<PlayerBulletHitCommandData, PlayerBulletHitCommand>
    {
        private readonly PlayerBulletHitCommandData _commandData;
        private readonly IEnemiesModule _enemiesModule;
        private readonly IScoreModule _scoreModule;
        private readonly IMainGameUiModule _mainGameUiModule;
        private readonly IPlayerBulletModule _playerBulletModule;

        public PlayerBulletHitCommand(PlayerBulletHitCommandData commandData, IEnemiesModule enemiesModule, IScoreModule scoreModule, IMainGameUiModule mainGameUiModule, IPlayerBulletModule playerBulletModule)
        {
            _commandData = commandData;
            _enemiesModule = enemiesModule;
            _scoreModule = scoreModule;
            _mainGameUiModule = mainGameUiModule;
            _playerBulletModule = playerBulletModule;
        }

        public override async UniTask Execute()
        {
            var enemyViewHit = _commandData.HitCollider2D.gameObject.GetComponent<EnemyView>();
            
            if (enemyViewHit == null)
            {
                return;
            }
            
            _scoreModule.AddScore(_enemyViewHit.);
            _mainGameUiModule.UpdateScore(_scoreModule.PlayerScore);
            _enemiesModule.EnemyHit(enemyViewHit);
            _playerBulletModule.BulletHit(_commandData.HitPlayerBulletView);
        }
    }
}