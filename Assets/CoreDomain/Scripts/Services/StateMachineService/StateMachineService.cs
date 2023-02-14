using Cysharp.Threading.Tasks;

namespace CoreDomain.Services.GameStates
{
    public class StateMachineService : IStateMachineService
    {
        private IGameState _currentGameState;

        public void EnterInitialGameState(IGameState initialState)
        {
            _currentGameState = initialState;
            _currentGameState.EnterState();
        }

        public async UniTask SwitchState(IGameState newState)
        {
            await _currentGameState.ExitState();
            _currentGameState = newState;
            await _currentGameState.EnterState();
        }
    }
}