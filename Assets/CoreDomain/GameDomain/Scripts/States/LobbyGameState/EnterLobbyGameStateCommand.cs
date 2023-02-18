using CoreDomain.GameDomain;
using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using CoreDomain.Scripts.Utils.Command;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreDomain.Services.GameStates
{
    public class EnterLobbyGameStateCommand : CommandOneParameter<LobbyGameStateEnterData, EnterLobbyGameStateCommand>
    {
        private readonly ILobbyUiModule _lobbyUiModule;
        private readonly LobbyGameStateEnterData _lobbyGameStateEnterData;
        private ILevelsService _levelsService;

        public EnterLobbyGameStateCommand(LobbyGameStateEnterData lobbyGameStateEnterData, ILobbyUiModule lobbyUiModule, ILevelsService levelsService)
        {
            _lobbyGameStateEnterData = lobbyGameStateEnterData;
            _lobbyUiModule = lobbyUiModule;
            _levelsService = levelsService;
        }

        public override async UniTask Execute()
        {
            Debug.Log("CreateLobby");
            var levelsAmount = _levelsService.GetLevelsAmount();
            _lobbyUiModule.CreateLobbyUi(levelsAmount);
        }
    }
}