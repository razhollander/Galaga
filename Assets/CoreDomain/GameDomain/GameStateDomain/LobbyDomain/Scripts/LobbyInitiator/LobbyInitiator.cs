using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class LobbyInitiator : MonoBehaviour
    {
        [SerializeField] private LobbyGameStateEnterData _defaultLobbyGameStateEnterData;
        
        private EnterLobbyGameStateCommand.Factory _enterLobbyGameStateCommand;
        
        [Inject]
        private void Setup(EnterLobbyGameStateCommand.Factory enterLobbyGameStateCommand)
        {
            _enterLobbyGameStateCommand = enterLobbyGameStateCommand;
        }

        public async UniTask StartState(LobbyGameStateEnterData lobbyGameStateEnterData = null)
        {
            var enterData = lobbyGameStateEnterData ?? _defaultLobbyGameStateEnterData;
            _enterLobbyGameStateCommand.Create(enterData).Execute();
        }
    }
}