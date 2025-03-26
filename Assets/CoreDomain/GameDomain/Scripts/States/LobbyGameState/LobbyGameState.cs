using CoreDomain.GameDomain.GameStateDomain.LobbyDomain;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.Services.GameStates
{
    public class LobbyGameState : BaseGameState<LobbyGameStateEnterData>
    {
        private ISceneLoaderService _sceneLoaderService;
        private DiContainer _container;
        public override GameStateType GameState => GameStateType.Lobby;

        public LobbyGameState(ISceneLoaderService sceneLoaderService, LobbyGameStateEnterData data = null) : base(data)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public override async UniTask EnterState()
        {
            await _sceneLoaderService.TryLoadScene(SceneName.Lobby);
            await GameObject.FindObjectOfType<LobbyInitiator>().EnterState(EnterData);
        }

        public override async UniTask ExitState()
        {
            await GameObject.FindObjectOfType<LobbyInitiator>().ExitState();
            await _sceneLoaderService.TryUnloadScene(SceneName.Lobby);
        }

        public class Factory : PlaceholderFactory<LobbyGameState>
        {
        }
    }
}