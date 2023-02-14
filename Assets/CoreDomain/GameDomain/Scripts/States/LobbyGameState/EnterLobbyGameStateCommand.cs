using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils.Command;
using Cysharp.Threading.Tasks;
using Services.Logs.Base;
using Zenject;

namespace CoreDomain.Services.GameStates
{
    public class EnterLobbyGameStateCommand : CommandOneParameter<LobbyGameStateEnterData, EnterLobbyGameStateCommand>
    {
        private readonly ILobbyUiModule _lobbyUiModule;
        private readonly LobbyGameStateEnterData _lobbyGameStateEnterData;

        public EnterLobbyGameStateCommand(LobbyGameStateEnterData lobbyGameStateEnterData, ILobbyUiModule lobbyUiModule)
        {
            _lobbyGameStateEnterData = lobbyGameStateEnterData;
            _lobbyUiModule = lobbyUiModule;
        }

        public override async UniTask Execute()
        {
            LogService.Log("CreateLobby");
            _lobbyUiModule.CreateLobbyUi();
        }
    }
}