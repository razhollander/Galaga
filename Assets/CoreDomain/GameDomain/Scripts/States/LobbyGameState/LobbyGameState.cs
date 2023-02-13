using Cysharp.Threading.Tasks;
using Zenject;

namespace CoreDomain.Services.GameStates
{
    public class LobbyGameState : BaseGameState<LobbyGameStateEnterData>
    {
        private EnterLobbyGameStateCommand.Factory _enterLobbyGameStateCommand;
        public override GameStateType GameState => GameStateType.Lobby;
        
        public LobbyGameState(LobbyGameStateEnterData data = null) :base(data)
        {
            
        }

        [Inject]
        private void InjectDependencies(EnterLobbyGameStateCommand.Factory enterLobbyGameStateCommand)
        {
            _enterLobbyGameStateCommand = enterLobbyGameStateCommand;
        }
        
        public override async UniTask EnterState()
        {
            await _enterLobbyGameStateCommand.Create(EnterData).Execute();
        }

        public override async UniTask ExitState()
        {
            //await _startScreenController.DestroyStartScreen();
        }
    }
}