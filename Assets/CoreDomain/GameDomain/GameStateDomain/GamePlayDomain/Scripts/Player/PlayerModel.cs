using System;
using Client;
using Features.MainGameScreen.GameLogicManager;
using Handlers;
using CoreDomain;
using UnityEngine;

namespace Features.MainGameScreen.Player
{
    public class PlayerModel : IDisposable, ISavableObject
    {
        #region --- Constants ---

        private const string PLAYER_SAVE_KAY = "PlayerSave";

        #endregion


        #region --- Events ---

        public event Action PlayerLoseEvent;

        #endregion


        #region --- Members ---

        public float PlayerSpeed = 5;
        public float PlayerStartXPosition;
        public float PlayerYPosRelativeToScreen;
        public Transform PlayerTransform;
        private readonly IClient _client;

        #endregion


        #region --- Properties ---

        public bool IsPlayerEnabled { get; private set; }

        #endregion


        #region --- Construction ---

        public PlayerModel(IClient client)
        {
            _client = client;
            PlayerYPosRelativeToScreen = -_client.CameraManager.ScreenBounds.y * 3 / 4;
            IsPlayerEnabled = true;

            AddListeners();
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            RemoveListeners();
        }

        public void Load()
        {
            PlayerStartXPosition = SaveLocallyHandler.LoadObject<PlayerData>(PLAYER_SAVE_KAY).XPosition;
        }

        public void Save()
        {
            SaveLocallyHandler.SaveObject(PLAYER_SAVE_KAY, new PlayerData(PlayerTransform.position.x));
        }

        #endregion


        #region --- Private Methods ---

        private void AddListeners()
        {
            _client.GameSaverService.RegisterSavedObject(this);
            _client.Broadcaster.Add<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Add<PlayerWinEvent>(OnPlayerWin);
        }

        private void RemoveListeners()
        {
            _client.GameSaverService.UnregisterSavedObject(this);
            _client.Broadcaster.Remove<PlayerLoseEvent>(OnPlayerLose);
            _client.Broadcaster.Remove<PlayerWinEvent>(OnPlayerWin);
        }

        private void SetPlayerDisabled()
        {
            IsPlayerEnabled = false;
        }

        #endregion


        #region --- Event Handler ---

        private void OnPlayerLose(PlayerLoseEvent obj)
        {
            SetPlayerDisabled();
            PlayerLoseEvent?.Invoke();
        }

        private void OnPlayerWin(PlayerWinEvent obj)
        {
            SetPlayerDisabled();
        }

        #endregion


        #region --- Inner Classes ---

        private class PlayerData
        {
            #region --- Members ---

            public readonly float XPosition;

            #endregion


            #region --- Construction ---

            public PlayerData(float xPosition)
            {
                XPosition = xPosition;
            }

            #endregion
        }

        #endregion
    }
}