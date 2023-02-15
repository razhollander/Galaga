using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services.GameStates;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class LobbyInitiator : MonoBehaviour
    {
        private CommandOneParameter<LobbyGameStateEnterData, EnterLobbyGameStateCommand>.Factory _enterLobbyGameStateCommand;
        private IStateMachineService _stateMachineService;

        [Inject]
        private void Setup(EnterLobbyGameStateCommand.Factory enterLobbyGameStateCommand, IStateMachineService stateMachineService)
        {
            _enterLobbyGameStateCommand = enterLobbyGameStateCommand;
            _stateMachineService = stateMachineService;
        }

        private void Start()
        {
            _stateMachineService.
            _enterLobbyGameStateCommand.Create().Execute();
        }
    }
}