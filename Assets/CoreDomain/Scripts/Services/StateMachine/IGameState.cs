using System;

namespace GameStates
{
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
}