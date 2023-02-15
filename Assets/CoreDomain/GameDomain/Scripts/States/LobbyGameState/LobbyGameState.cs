using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Scripts.Services.SceneService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.Services.GameStates
{
    public class LobbyGameState : BaseGameState<LobbyGameStateEnterData>
    {
        private EnterLobbyGameStateCommand.Factory _enterLobbyGameStateCommand;
        private ISceneLoaderService _sceneLoaderService;
        private DiContainer _container;
        public override GameStateType GameState => GameStateType.Lobby;

        public LobbyGameState(LobbyGameStateEnterData data = null) : base(data)
        {
        }

        public override async UniTask EnterState()
        {
            await _sceneLoaderService.TryLoadScene(SceneName.Lobby);
            await UniTask.Yield();
            Debug.Log("CreateCommand");
            // var lobbyUiModule = _container.Resolve<ILobbyUiModule>();
            // if (lobbyUiModule == null)
            // {
            //     Debug.Log("State null");
            // }
            // else
            // {
            //     Debug.Log("State not null");
            // }
            await _enterLobbyGameStateCommand.Create(EnterData).Execute();
        }

        public override async UniTask ExitState()
        {
            await _sceneLoaderService.TryUnloadScene(SceneName.Lobby);
        }

        [Inject]
        private void Construct(ISceneLoaderService sceneLoaderService, EnterLobbyGameStateCommand.Factory enterLobbyGameStateCommand, DiContainer container)
        {
            _sceneLoaderService = sceneLoaderService;
            _enterLobbyGameStateCommand = enterLobbyGameStateCommand;
            _container = container;
            container.
        }

        public class Factory : PlaceholderFactory<LobbyGameState>
        {
        }
    }
}