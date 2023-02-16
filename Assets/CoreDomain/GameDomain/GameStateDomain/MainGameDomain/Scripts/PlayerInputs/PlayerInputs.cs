using System;
using Client;
using CoreDomain;
using UnityEngine.InputSystem;

namespace Systems
{
    public class PlayerInputs : IDisposable, IUpdatable
    {
        public event Action ShootEvent;
        public event Action<float> MoveChangeEvent;

        private float _playerMoveValue;
        private readonly GameInputActions _gameInputActions;
        private readonly IClient _client;

        public PlayerInputs(IClient client)
        {
            _client = client;
            _gameInputActions = _client.GameInputActions;

            AddListeners();
        }
        
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

        private void AddListeners()
        {
            _gameInputActions.BaseSpace.Shoot.started += OnPlayerShoot;
            _client.UpdateSubscriptionService.RegisterUpdatable(this);
        }

        private void RemoveListeners()
        {
            _gameInputActions.BaseSpace.Shoot.started -= OnPlayerShoot;
            _client.UpdateSubscriptionService.UnregisterUpdatable(this);
        }
        
        
        private void OnPlayerMove()
        {
            MoveChangeEvent?.Invoke(_playerMoveValue);
        }

        private void OnPlayerShoot(InputAction.CallbackContext context)
        {
            ShootEvent?.Invoke();
        }
    }
}