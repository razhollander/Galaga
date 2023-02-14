using System.Collections;
using System.Collections.Generic;
using CoreDomain.Services.GameStates;
using UnityEngine;
using Zenject;

public class GameInitiator : MonoBehaviour
{
    private IStateMachineService _stateMachine;
    private LobbyGameState.Factory _lobbyGameStateFactory;
    
    [Inject]
    private void Setup(IStateMachineService stateMachine, LobbyGameState.Factory lobbyGameStateFactory)
    {
        _stateMachine = stateMachine;
        _lobbyGameStateFactory = lobbyGameStateFactory;
    }
        
    private void Start()
    {
        UpdateApplicationSettings();
        EnterLobbyState();
    }

    private void EnterLobbyState()
    {
        _stateMachine.EnterInitialGameState(_lobbyGameStateFactory.Create());
    }

    private void UpdateApplicationSettings()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
