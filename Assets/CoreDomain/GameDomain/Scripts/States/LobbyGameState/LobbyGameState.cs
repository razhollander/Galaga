using CoreDomain.Scripts.Services.SceneService;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CoreDomain.Services.GameStates
{
    public class LobbyGameState : BaseGameState<LobbyGameStateEnterData>
    {
        private EnterLobbyGameStateCommand.Factory _enterLobbyGameStateCommand;
        private ISceneLoaderService _sceneLoaderService;
        public override GameStateType GameState => GameStateType.Lobby;

        public LobbyGameState(LobbyGameStateEnterData data = null) : base(data)
        {
        }

        public override async UniTask EnterState()
        {
            await _sceneLoaderService.TryLoadScene(SceneName.Lobby);
        }

        public override async UniTask ExitState()
        {
            await _sceneLoaderService.TryUnloadScene(SceneName.Lobby);
        }

        [Inject]
        private void Construct(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public class Factory : PlaceholderFactory<LobbyGameState>
        {
        }
    }
}