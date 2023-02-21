using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Score;
using CoreDomain.Services;
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
        private readonly IAudioService _audioService;

        public PlayerBulletHitCommand(PlayerBulletHitCommandData commandData, IEnemiesModule enemiesModule, IScoreModule scoreModule, IMainGameUiModule mainGameUiModule, IPlayerBulletModule playerBulletModule, IAudioService audioService)
        {
            _commandData = commandData;
            _enemiesModule = enemiesModule;
            _scoreModule = scoreModule;
            _mainGameUiModule = mainGameUiModule;
            _playerBulletModule = playerBulletModule;
            _audioService = audioService;
        }

        public override void Execute()
        {
            var enemyViewHit = _commandData.HitCollider2D.gameObject.GetComponent<EnemyView>();
            
            if (enemyViewHit == null || _commandData.HitPlayerBulletView == null)
            {
                return;
            }
            
            var enemyHitId = enemyViewHit.Id;
            var bulletId = _commandData.HitPlayerBulletView.Id;
           
            // in unity physics, the same bullet can hit multiple enemies at the same moment, so need to check that the hit didn't happen already
            var isFirstTimeHitHappens = _enemiesModule.IsEnemyExist(enemyHitId) && _playerBulletModule.IsBulletExist(bulletId);
            
            if (isFirstTimeHitHappens)
            {
                _scoreModule.AddScore(_enemiesModule.GetEnemyScore(enemyHitId));
                _mainGameUiModule.UpdateScore(_scoreModule.PlayerScore);
                _enemiesModule.KillEnemy(enemyHitId);
                _playerBulletModule.DestroyBullet(bulletId);
                _audioService.PlayAudio(AudioClipName.HitSoundFXName, AudioChannelType.Fx, AudioPlayType.OneShot);
            }
        }
    }
}