using System;
using Client;
using Managers;
using UnityEngine.InputSystem;

namespace Systems
{
    public class PlayerInputs : IDisposable, UpdateManager.IUpdatable
    {
        #region --- Events ---

        public event Action ShootEvent;

        public event Action<float> MoveChangeEvent;

        #endregion


        #region --- Members ---

        private float _playerMoveValue;
        private readonly GameInputActions _gameInputActions;
        private readonly IClient _client;

        #endregion


        #region --- Construction ---

        public PlayerInputs(IClient client)
        {
            _client = client;
            _gameInputActions = _client.GameInputActions;

            AddListeners();
        }

        #endregion


        #region --- Public Methods ---

        public void Dispose()
        {
            RemoveListeners();
        }

        public void ManagedUpdate()
        {
            _playerMoveValue = _gameInputActions.BaseSpace.Move.ReadValue<float>();

            if (_playerMoveValue > 0 || _playerMoveValue < 0) // dont want to compare floats (_playerMoveValue != 0)
            {
                OnPlayerMove();
            }
        }

        #endregion


        #region --- Private Methods ---

        private void AddListeners()
        {
            _gameInputActions.BaseSpace.Shoot.started += OnPlayerShoot;
            _client.UpdateManager.RegisterUpdatable(this);
        }

        private void RemoveListeners()
        {
            _gameInputActions.BaseSpace.Shoot.started -= OnPlayerShoot;
            _client.UpdateManager.UnregisterUpdatable(this);
        }

        #endregion


        #region --- Event Handler ---

        private void OnPlayerMove()
        {
            MoveChangeEvent?.Invoke(_playerMoveValue);
        }

        private void OnPlayerShoot(InputAction.CallbackContext context)
        {
            ShootEvent?.Invoke();
        }

        #endregion
    }
}