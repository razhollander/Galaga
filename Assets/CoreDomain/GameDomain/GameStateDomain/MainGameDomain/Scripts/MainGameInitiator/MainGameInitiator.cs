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

        [Inject]
        private void Setup(IMainGameUiModule mainGameUiModule, IPlayerSpaceshipModule playerSpaceshipModule)
        {
            _mainGameUiModule = mainGameUiModule;
            _playerSpaceshipModule = playerSpaceshipModule;
        }

        public async UniTask StartState(MainGameStateEnterData mainGameStateEnterData)
        {
            var enterData = mainGameStateEnterData ?? _defaultLobbyGameStateEnterData;
            _mainGameUiModule.CreateMainGameUi();
            _playerSpaceshipModule.CreatePlayerSpaceship(enterData.PlayerName);
        }
    }
}