using Cysharp.Threading.Tasks;
using Services.Logs.Base;
using Zenject;

namespace CoreDomain.Services.GameStates
{
    [ZenjectAllowDuringValidation]
    public class LobbyGameState : BaseGameState<LobbyGameStateEnterData>
    {
        private EnterLobbyGameStateCommand.Factory _enterLobbyGameStateCommand;
        public override GameStateType GameState => GameStateType.Lobby;
        
        public LobbyGameState(LobbyGameStateEnterData data = null) :base(data)
        {
            LogService.Log("Constructor");
        }

        [Inject]
        private void Construct(EnterLobbyGameStateCommand.Factory enterLobbyGameStateCommand)
        {
            LogService.Log("Inject");
            _enterLobbyGameStateCommand = enterLobbyGameStateCommand;
        }
        
        public override async UniTask EnterState()
        {
            LogService.Log("EnterState");
            
            if (_enterLobbyGameStateCommand == null)
            {
                LogService.Log("null");
            }
            await _enterLobbyGameStateCommand.Create(EnterData).Execute();
        }

        public override async UniTask ExitState()
        {
            //await _startScreenController.DestroyStartScreen();
        }
        
        public class Factory : PlaceholderFactory<LobbyGameState>
        {
        }
    }
}