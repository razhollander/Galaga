using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public class LobbyUiViewModule
    {
        private LobbyUiView _lobbyUiView;

        public void SetupLobbyUiView(LobbyUiView lobbyUiView)
        {
            _lobbyUiView = lobbyUiView;
        }

        public void DestroyLobbyUiView()
        {
            Object.Destroy(_lobbyUiView.gameObject);
        }
    }
}
