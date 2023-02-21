using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
    public class StateMachineService : IStateMachineService
    {
        private IGameState _currentGameState;

        public IGameState CurrentState()
        {
            return _currentGameState;
        }

        public void EnterInitialGameState(IGameState initialState)
        {
            _currentGameState = initialState;
            _currentGameState.EnterState();
        }

        public async UniTask SwitchState(IGameState newState)
        {
            if (_currentGameState == null)
            {
                LogService.LogError("No state to switch from, need to initialize a game state first!");
                return;
            }
            
            await _currentGameState.ExitState();
            _currentGameState = newState;
            await _currentGameState.EnterState();
        }
    }
}