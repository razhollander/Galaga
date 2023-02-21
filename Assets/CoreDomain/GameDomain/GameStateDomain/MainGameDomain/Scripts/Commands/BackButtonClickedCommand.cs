using CoreDomain.Scripts.Utils.Command;
using CoreDomain.Services.GameStates;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Commands
{
    public class BackButtonClickedCommand : CommandSync<BackButtonClickedCommand>
    {
        private readonly IStateMachineService _stateMachineService;
        private LobbyGameState.Factory _lobbyGameStateFactory;

        public BackButtonClickedCommand(IStateMachineService stateMachineService, LobbyGameState.Factory lobbyGameStateFactory)
        {
            _stateMachineService = stateMachineService;
            _lobbyGameStateFactory = lobbyGameStateFactory;
        }

        public override void Execute()
        {
            _stateMachineService.SwitchState(_lobbyGameStateFactory.Create());
        }
    }
}