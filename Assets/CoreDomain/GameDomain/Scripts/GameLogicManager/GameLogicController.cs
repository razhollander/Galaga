using System;
using Client;
using Popups.DefeatPopup;
using Popups.WinPopup;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.MainGameScreen.GameLogicManager
{
    public class GameLogicController : IDisposable
    {
        #region --- Members ---

        private bool _isGameEnabled = true;
        private readonly GameObject _gameSaver;
        private readonly IClient _client;

        #endregion


        #region --- Construction ---

        public GameLogicController(IClient client)
        {
            _client = client;
            _gameSaver = new GameObject();
            _gameSaver.AddComponent<GameProgressSaver>().Setup(OnApplicationQuit);

            AddListeners();
        }

        #endregion


        #region --- Mono Override ---

        private void OnApplicationQuit()
        {
            if (_isGameEnabled)
            {
                _client.GameSaverService.SaveGameData();
            }
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            Object.Destroy(_gameSaver);
            RemoveListeners();
        }

        #endregion


        #region --- Private Methods ---

        private void AddListeners()
        {
            _client.Broadcaster.Add<PlayerWinEvent>(OnPlayerWin);
            _client.Broadcaster.Add<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Add<PlayerDieEndEvent>(OnPlayerDieEnd);
        }

        private void RemoveListeners()
        {
            _client.Broadcaster.Remove<PlayerWinEvent>(OnPlayerWin);
            _client.Broadcaster.Remove<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Remove<PlayerDieEndEvent>(OnPlayerDieEnd);
        }

        #endregion


        #region --- Event Handler ---

        private void OnPlayerDieEnd(PlayerDieEndEvent obj)
        {
            //_client.PopupsManager.QueuePopup(new DefeatBasePopupController());
        }

        private void OnPlayerLose(PlayerLoseEvent obj)
        {
            _isGameEnabled = false;
        }

        private void OnPlayerWin(PlayerWinEvent obj)
        {
            //_client.PopupsManager.QueuePopup(new WinPopupController());
            _isGameEnabled = false;
        }

        #endregion
    }

    public class PlayerWinEvent
    {
    }

    public class PlayerLoseEvent
    {
    }

    public class PlayerDieEndEvent
    {
    }
}