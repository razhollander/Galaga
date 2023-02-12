using GameStates;

namespace CoreDomain.Services
{
    public interface IStateMachine
    {
        void SwitchState(IGameState newState);
        public void SetupInitialGameState(IGameState initialState);
    }
}