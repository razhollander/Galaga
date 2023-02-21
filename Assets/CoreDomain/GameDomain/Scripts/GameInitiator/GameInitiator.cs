using CoreDomain.Services.GameStates;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain
{
    public class GameInitiator : MonoBehaviour
    {
        private IStateMachineService _stateMachine;
        private LobbyGameState.Factory _lobbyGameStateFactory;
        private ILevelsService _levelsService;

        [Inject]
        private void Setup(IStateMachineService stateMachine, LobbyGameState.Factory lobbyGameStateFactory, ILevelsService levelsService)
        {
            _stateMachine = stateMachine;
            _lobbyGameStateFactory = lobbyGameStateFactory;
            _levelsService = levelsService;
        }

        private void Start()
        {
            UpdateApplicationSettings();
            InitializeServices();
            EnterToLobbyGameState();
        }

        private void InitializeServices()
        {
            _levelsService.LoadLevels();
        }

        private void EnterToLobbyGameState()
        {
            _stateMachine.EnterInitialGameState(_lobbyGameStateFactory.Create());
        }

        private void UpdateApplicationSettings()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }
    }
}