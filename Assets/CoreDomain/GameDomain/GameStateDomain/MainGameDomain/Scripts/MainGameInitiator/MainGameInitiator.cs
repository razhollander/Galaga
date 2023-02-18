using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.Enemies;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.PlayerSpaceship;
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

        [Inject]
        private void Setup(IMainGameUiModule mainGameUiModule, IPlayerSpaceshipModule playerSpaceshipModule, ILevelsService levelsService, IEnemiesModule enemiesModule)
        {
            _mainGameUiModule = mainGameUiModule;
            _playerSpaceshipModule = playerSpaceshipModule;
            _levelsService = levelsService;
            _enemiesModule = enemiesModule;
        }

        public async UniTask StartState(MainGameStateEnterData mainGameStateEnterData)
        {
            var enterData = mainGameStateEnterData ?? _defaultLobbyGameStateEnterData;
            _mainGameUiModule.CreateMainGameUi();
            _playerSpaceshipModule.CreatePlayerSpaceship(enterData.PlayerName);
            var levelData = _levelsService.GetLevel(enterData.Level);
            _enemiesModule.DoEnemiesWavesSequence(levelData.EnemiesWaveSequenceData);
        }
    }
}