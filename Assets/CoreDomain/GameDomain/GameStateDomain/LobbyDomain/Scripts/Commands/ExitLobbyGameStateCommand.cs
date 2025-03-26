using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Scripts.Utils.Command;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class ExitLobbyGameStateCommand : CommandSync<ExitLobbyGameStateCommand>
    {
        private readonly ILobbyUiModule _lobbyUiModule;

        public ExitLobbyGameStateCommand(ILobbyUiModule lobbyUiModule)
        {
            _lobbyUiModule = lobbyUiModule;
        }

        public override void Execute()
        {
            _lobbyUiModule.DestroyLobbyUi();
        }
    }
}