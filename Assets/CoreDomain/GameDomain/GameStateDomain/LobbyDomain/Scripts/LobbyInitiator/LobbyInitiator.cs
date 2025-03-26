using CoreDomain.Services.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class LobbyInitiator : MonoBehaviour , IStateInitiator<LobbyGameStateEnterData>
    {
        [SerializeField] private LobbyGameStateEnterData _defaultLobbyGameStateEnterData;
        
        private EnterLobbyGameStateCommand.Factory _enterLobbyGameStateCommand;
        private ExitLobbyGameStateCommand.Factory _exitLobbyGameStateCommandFactory;

        [Inject]
        private void Setup(EnterLobbyGameStateCommand.Factory enterLobbyGameStateCommand, ExitLobbyGameStateCommand.Factory exitLobbyGameStateCommandFactory)
        {
            _enterLobbyGameStateCommand = enterLobbyGameStateCommand;
            _exitLobbyGameStateCommandFactory = exitLobbyGameStateCommandFactory;
        }

        public async UniTask EnterState(LobbyGameStateEnterData lobbyGameStateEnterData = null)
        {
            var enterData = lobbyGameStateEnterData ?? _defaultLobbyGameStateEnterData;
            _enterLobbyGameStateCommand.Create(enterData).Execute();
        }
        
        public async UniTask ExitState()
        {
            _exitLobbyGameStateCommandFactory.Create().Execute();
        }
        
        private void OnApplicationQuit()
        {
            ExitState().Forget();
        }
    }
}