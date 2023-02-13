using CoreDomain.Services;
using CoreDomain.Services.GameStates;
using Services.Logs.Base;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameInitiator
{
    public class GameInitiator : MonoBehaviour
    {
        private IStateMachineService _stateMachine;
        private GameInputActions _gameInputActions;
        
        [Inject]
        private void Setup(IStateMachineService stateMachine, GameInputActions gameInputActions)
        {
            _stateMachine = stateMachine;
            _gameInputActions = gameInputActions;
        }
        
        private void Start()
        {
            InitializeServices();
            UpdateApplicationSettings();
            EnterLobbyState();
        }

        private void EnterLobbyState()
        {
            _stateMachine.EnterInitialGameState(new LobbyGameState());
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