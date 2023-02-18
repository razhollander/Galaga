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

        public void DestroyMainGameUiView()
        {
            Object.Destroy(_mainGameUiView.gameObject);
        }

        public void UpdateScore(int newScore)
        {
            _mainGameUiView.UpdateScore(newScore);
        }
    }
}
