using System;
using Client;
using Features.MainGameScreen.Enemy;
using Features.MainGameScreen.GameLogicManager;
using Features.MainGameScreen.Player;

namespace GameStates
{
    public class MainGameState : IGameState
    {
        #region --- Members ---

        private readonly bool _isFromLastSave;
        private readonly EnemiesController _enemiesController;
        private readonly GameLogicController _gameLogicController;
        private readonly IClient _client;
        private readonly PlayerController _playerController;

        #endregion


        #region --- Properties ---

        public eGameState StateType
        {
            get { return eGameState.MainGameState; }
        }

        #endregion


        #region --- Construction ---

        public MainGameState(bool isFromLastSave = false)
        {
            _client = Client.Client.Instance;
            _isFromLastSave = isFromLastSave;
            _gameLogicController = new GameLogicController(_client);
            _playerController = new PlayerController(_client);
            _enemiesController = new EnemiesController(_client);
        }

        #endregion


        #region --- Public Methods ---

        public void EnterState()
        {
            if (_isFromLastSave)
            {
                _client.GameSaverService.LoadGameData();
            }

            _playerController.Setup();
            _enemiesController.Setup();
        }

        public void ExitState(Action<IGameState> onExitDone)
        {
            _gameLogicController.Dispose();
            _playerController.Dispose();
            _enemiesController.Dispose();
            onExitDone(this);
        }

        public void IdleState()
        {
        }

        #endregion
    }
}