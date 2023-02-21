using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerBullet;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship;
using CoreDomain.Services;
using CoreDomain.Services.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain
{
    public class MainGameInitiator : MonoBehaviour
    {
        [SerializeField] private MainGameStateEnterData _defaultLobbyGameStateEnterData;

        private IMainGameUiModule _mainGameUiModule;
        private IPlayerSpaceshipModule _playerSpaceshipModule;
        private ILevelsService _levelsService;
        private IEnemiesModule _enemiesModule;
        private IAudioService _audioService;
        private IPlayerBulletModule _playerBulletModule;

        [Inject]
        private void Setup(IMainGameUiModule mainGameUiModule, IPlayerSpaceshipModule playerSpaceshipModule, ILevelsService levelsService, IEnemiesModule enemiesModule, IAudioService audioService, IPlayerBulletModule playerBulletModule)
        {
            _mainGameUiModule = mainGameUiModule;
            _playerSpaceshipModule = playerSpaceshipModule;
            _levelsService = levelsService;
            _enemiesModule = enemiesModule;
            _audioService = audioService;
            _playerBulletModule = playerBulletModule;
        }

        public async UniTask StartState(MainGameStateEnterData mainGameStateEnterData)
        {
            var enterData = mainGameStateEnterData ?? _defaultLobbyGameStateEnterData;
            _mainGameUiModule.CreateMainGameUi();
            _playerSpaceshipModule.CreatePlayerSpaceship(enterData.PlayerName);
            var levelData = _levelsService.GetLevelData(enterData.Level);
            _enemiesModule.StartEnemiesWavesSequence(levelData.EnemiesWaveSequenceData);
            _audioService.PlayAudio(AudioClipName.ThemeSongName, AudioChannelType.Master, AudioPlayType.Loop);
        }
        
        public async UniTask DisposeState(MainGameStateEnterData mainGameStateEnterData)
        {
            _mainGameUiModule.Dispose();
            _playerSpaceshipModule.Dispose();
            _enemiesModule.Dispose();
            _playerBulletModule.Dispose();
            _audioService.StopAllSounds();
        }
    }
}