using CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain
{
    public class MainGameInitiator : MonoBehaviour
    {
        private IMainGameUiModule _lobbyUiModule;

        [Inject]
        private void Setup(IMainGameUiModule lobbyUiModule)
        {
            _lobbyUiModule = lobbyUiModule;
        }

        private void Start()
        {
            _lobbyUiModule.CreateMainGameUi();
        }
    }
}