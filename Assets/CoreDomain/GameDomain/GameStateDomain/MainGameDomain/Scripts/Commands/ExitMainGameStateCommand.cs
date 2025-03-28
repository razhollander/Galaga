using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship;
using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services;
using Cysharp.Threading.Tasks;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain
{
    public class ExitMainGameStateCommand : Command<ExitMainGameStateCommand>
    {
        private readonly IMainGameUiModule _mainGameUiModule;
        private readonly IPlayerSpaceshipModule _playerSpaceshipModule;
        private readonly IEnemiesModule _enemiesModule;
        private readonly IAudioService _audioService;
        private readonly IPlayerBulletModule _playerBulletModule;

        public ExitMainGameStateCommand(IMainGameUiModule mainGameUiModule, IPlayerSpaceshipModule playerSpaceshipModule, IEnemiesModule enemiesModule, IAudioService audioService, IPlayerBulletModule playerBulletModule)
        {
            _mainGameUiModule = mainGameUiModule;
            _playerSpaceshipModule = playerSpaceshipModule;
            _enemiesModule = enemiesModule;
            _audioService = audioService;
            _playerBulletModule = playerBulletModule;
        }

        public override async UniTask Execute()
        {
            _mainGameUiModule.Dispose();
            _playerSpaceshipModule.Dispose();
            _enemiesModule.Dispose();
            _playerBulletModule.Dispose();
            _audioService.StopAllSounds();
        }
    }
}