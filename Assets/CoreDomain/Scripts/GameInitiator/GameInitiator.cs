using CoreDomain.Services;
using GameStates;
using Services.Logs.Base;
using UnityEngine;
using Zenject;

namespace CoreDomain.Scripts.GameInitiator
{
    public class GameInitiator : MonoBehaviour
    {
        private IStateMachine _stateMachine;
        private GameInputActions _gameInputActions;
        
        [Inject]
        private void Setup(IStateMachine stateMachine, GameInputActions gameInputActions)
        {
            _stateMachine = stateMachine;
            _gameInputActions = gameInputActions;
        }
        
        private void Awake()
        {
            InitializeServices();
            UpdateApplicationSettings();
            LoadState();
        }

        private void LoadState()
        {
            _stateMachine.SetupInitialGameState(new StartScreenState());
        }

        private void UpdateApplicationSettings()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
        
        private void InitializeServices()
        {
            _gameInputActions.Enable();
        }
    }
}