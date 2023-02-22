using System;
using CoreDomain.GameDomain.GameStateDomain.LobbyDomain;
using CoreDomain.Services.GameStates;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain
{
    public class MainGameInitiator : MonoBehaviour, IStateInitiator<MainGameStateEnterData>
    {
        [SerializeField] private MainGameStateEnterData _defaultLobbyGameStateEnterData;

        private EnterMainGameStateCommand.Factory _mainGameStateCommandFactory;
        private ExitMainGameStateCommand.Factory _exitMainGameStateCommandFactory;

        [Inject]
        private void Setup(EnterMainGameStateCommand.Factory mainGameStateCommandFactory, ExitMainGameStateCommand.Factory exitMainGameStateCommandFactory)
        {
            _mainGameStateCommandFactory = mainGameStateCommandFactory;
            _exitMainGameStateCommandFactory = exitMainGameStateCommandFactory;
        }
        
        public async UniTask EnterState(MainGameStateEnterData stateEnterData = null)
        {
            var enterData = stateEnterData ?? _defaultLobbyGameStateEnterData;
            await _mainGameStateCommandFactory.Create(enterData).Execute();
        }

        public async UniTask ExitState()
        {
            await _exitMainGameStateCommandFactory.Create().Execute();
        }

        private void OnApplicationQuit()
        {
            ExitState().Forget();
        }
    }
}