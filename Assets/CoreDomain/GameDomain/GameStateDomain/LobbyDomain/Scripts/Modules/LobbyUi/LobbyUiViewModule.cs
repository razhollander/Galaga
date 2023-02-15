using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public class LobbyUiViewModule
    {
        private LobbyUiView _lobbyUiView;

        public void SetupLobbyUiView(LobbyUiView lobbyUiView, int levels)
        {
            _lobbyUiView = lobbyUiView;
            _lobbyUiView.SetLevelsDropBox(levels);
        }

        public string GetPlayerName()
        {
            return _lobbyUiView.PlayerNameText;
        }
        
        public void DestroyLobbyUiView()
        {
            Object.Destroy(_lobbyUiView.gameObject);
        }
    }
}
