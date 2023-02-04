using System;
using Client;

namespace GameStates
{
    public class StateMachine
    {
        #region --- Properties ---

        public IClient Client { get; }

        public IGameState CurrentStateObject { get; private set; }

        #endregion


        #region --- Construction ---

        public StateMachine(IClient client, IGameState initialState)
        {
            Client = client;
            SetupInitialGameState(initialState);
        }

        #endregion


        #region --- Public Methods ---

        public void SwitchState(IGameState newState)
        {
            CurrentStateObject.ExitState(exitingState => OnStateExitDone(exitingState, newState));
        }

        #endregion


        #region --- Private Methods ---

        private void SetupInitialGameState(IGameState initialState)
        {
            CurrentStateObject = initialState;
            CurrentStateObject.EnterState();
            Client.Broadcaster.Broadcast(new OnStateEnter {State = initialState});
        }

        #endregion


        #region --- Event Handler ---

        private void OnStateExitDone(IGameState exitingState, IGameState enteringState)
        {
            Client.Broadcaster.Broadcast(new OnStateExit {State = CurrentStateObject});
            CurrentStateObject = enteringState;

            CurrentStateObject.EnterState();
            Client.Broadcaster.Broadcast(new OnStateEnter {State = enteringState});
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

    public interface IGameState
    {
        #region --- Properties ---

        eGameState StateType { get; }

        #endregion


        #region --- Public Methods ---

        void EnterState();
        void ExitState(Action<IGameState> onExitDone);
        void IdleState();

        #endregion
    }

    public enum eGameState
    {
        StartScreenState,
        MainGameState
    }
}