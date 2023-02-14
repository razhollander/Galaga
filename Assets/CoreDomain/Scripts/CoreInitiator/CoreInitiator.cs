using CoreDomain.Services;
using CoreDomain.Services.GameStates;
using Services.Logs.Base;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameInitiator
{
    public class CoreInitiator : MonoBehaviour
    {
        private GameInputActions _gameInputActions;
        
        [Inject]
        private void Setup(GameInputActions gameInputActions)
        {
            _gameInputActions = gameInputActions;
            InitializeServices();
        }

        private void InitializeServices()
        {
            _gameInputActions.Enable();
        }
    }
}