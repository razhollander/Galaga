using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
    public class LobbyGameState : IGameState
    {
        public GameStateType GameState => GameStateType.Lobby;
        
        public LobbyGameState()
        {
            
        }
        
        public async UniTask EnterState()
        {
            
            //await _startScreenController.CreateStartScreen();
        }

        public async UniTask ExitState()
        {
            //await _startScreenController.DestroyStartScreen();
        }
    }
}