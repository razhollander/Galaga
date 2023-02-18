using System;
using Client;
using Features.MainGameScreen.GameLogicManager;
using Handlers;
using CoreDomain;
using UnityEngine;

namespace Features.MainGameScreen.Player
{
    public class PlayerModel : IDisposable
    {
        private const string PLAYER_SAVE_KAY = "PlayerSave";
        
        public event Action PlayerLoseEvent;
        
        public float PlayerSpeed = 5;
        public float PlayerStartXPosition;
        public float PlayerYPosRelativeToScreen;
        public Transform PlayerTransform;
        private readonly IClient _client;
        
        public bool IsPlayerEnabled { get; private set; }
        
        public PlayerModel(IClient client)
        {
            _client = client;
            PlayerYPosRelativeToScreen = -_client.CameraManager.ScreenBounds.y * 3 / 4;
            IsPlayerEnabled = true;

            AddListeners();
        }
        
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

        private void SetPlayerDisabled()
        {
            IsPlayerEnabled = false;
        }
        
        private void OnPlayerLose(PlayerLoseEvent obj)
        {
            SetPlayerDisabled();
            PlayerLoseEvent?.Invoke();
        }

        private void OnPlayerWin(PlayerWinEvent obj)
        {
            SetPlayerDisabled();
        }
        
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
    }
}