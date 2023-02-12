using GameStates;

namespace CoreDomain.Services
{
    public class StateMachine : IStateMachine
    {
        #region --- Properties ---
        
        private IGameState CurrentStateObject { get; set; }

        #endregion


        #region --- Public Methods ---

        public void SwitchState(IGameState newState)
        {
            CurrentStateObject.ExitState(exitingState => OnStateExitDone(exitingState, newState));
        }

        #endregion


        #region --- Private Methods ---

        public void SetupInitialGameState(IGameState initialState)
        {
            CurrentStateObject = initialState;
            CurrentStateObject.EnterState();
        }

        #endregion


        #region --- Event Handler ---

        private void OnStateExitDone(IGameState exitingState, IGameState enteringState)
        {
            // Client.Broadcaster.Broadcast(new OnStateExit {State = CurrentStateObject});
            CurrentStateObject = enteringState;

            CurrentStateObject.EnterState();
            // Client.Broadcaster.Broadcast(new OnStateEnter {State = enteringState});
        }

        #endregion


        #region --- Inner Classes ---

        public class OnStateChangedEvent
        {
            #region --- Members ---

            public IGameState State;

            #endregion
        }

        public class OnStateEnter : OnStateChangedEvent
        {
        }

        public class OnStateExit : OnStateChangedEvent
        {
        }

        #endregion
    }
}