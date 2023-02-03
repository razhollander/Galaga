using System;
using Features.StartScreen;
using GameStates;

namespace GameStates
{
    public class StartScreenState : IGameState
    {
        #region --- Members ---

        private readonly StartScreenController _startScreenController;

        #endregion


        #region --- Properties ---

        public eGameState StateType
        {
            get { return eGameState.StartScreenState; }
        }

        #endregion


        #region --- Construction ---

        public StartScreenState()
        {
            _startScreenController = new StartScreenController();
        }

        #endregion


        #region --- Public Methods ---

        public void EnterState()
        {
            _startScreenController.CreateStartScreen();
        }

        public void ExitState(Action<IGameState> onExitDone)
        {
            _startScreenController.DestroyStartScreen();
            onExitDone(this);
        }

        public void IdleState()
        {
        }

        #endregion
    }
}