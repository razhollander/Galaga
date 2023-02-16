using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi
{
    public class MainGameUiViewModule
    {
        private MainGameUiView _mainGameUiView;

        public void SetupMainGameUiView(MainGameUiView mainGameUiView)
        {
            _mainGameUiView = mainGameUiView;
        }

        // public string GetPlayerName()
        // {
        //     return _mainGameUiView.PlayerNameText;
        // }
        
        public void DestroyMainGameUiView()
        {
            Object.Destroy(_mainGameUiView.gameObject);
        }
    }
}
