using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Score;
using CoreDomain.Scripts.Utils.Command;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands
{
    public class PlayerBulletHitCommand : CommandSyncOneParameter<PlayerBulletHitCommandData, PlayerBulletHitCommand>
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

        public override void Execute()
        {
            var enemyViewHit = _commandData.HitCollider2D.gameObject.GetComponent<EnemyView>();
            
            if (enemyViewHit == null)
            {
                return;
            }
            
            _scoreModule.AddScore(_enemiesModule.GetEnemyScore(enemyViewHit.Id));
            _mainGameUiModule.UpdateScore(_scoreModule.PlayerScore);
            _enemiesModule.KillEnemy(enemyViewHit.Id);
            _playerBulletModule.DestroyBullet(_commandData.HitPlayerBulletView.Id);
        }
    }
}