using CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain
{
    public class LobbyInitiator : MonoBehaviour
    {
        private ILobbyUiModule _lobbyUiModule;

        [Inject]
        private void Setup(ILobbyUiModule lobbyUiModule)
        {
            _lobbyUiModule = lobbyUiModule;
        }

        private void Start()
        {
            _lobbyUiModule.CreateLobbyUi();
        }
    }
}