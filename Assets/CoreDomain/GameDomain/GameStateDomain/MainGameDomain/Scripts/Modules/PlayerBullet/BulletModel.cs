using System;
using Client;
using Features.MainGameScreen.GameLogicManager;

namespace Features.MainGameScreen.Bullet
{
    public class BulletModel : IDisposable
    {
        #region --- Members ---

        public float BulletSpeed = 10;
        private readonly IClient _client;

        #endregion


        #region --- Properties ---

        public bool IsBulletEnabled { get; private set; }

        #endregion


        #region --- Construction ---

        public BulletModel(IClient client)
        {
            _client = client;
            IsBulletEnabled = true;
            AddListeners();
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            RemoveListeners();
        }

        #endregion


        #region --- Private Methods ---

        private void DisableBullet()
        {
            IsBulletEnabled = false;
        }

        private void AddListeners()
        {
            _client.Broadcaster.Add<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Add<PlayerWinEvent>(OnPlayerWin);
        }

        private void RemoveListeners()
        {
            _client.Broadcaster.Remove<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Remove<PlayerWinEvent>(OnPlayerWin);
        }

        #endregion


        #region --- Event Handler ---

        private void OnPlayerLose(PlayerLoseEvent obj)
        {
            DisableBullet();
        }

        private void OnPlayerWin(PlayerWinEvent obj)
        {
            DisableBullet();
        }

        #endregion
    }
}