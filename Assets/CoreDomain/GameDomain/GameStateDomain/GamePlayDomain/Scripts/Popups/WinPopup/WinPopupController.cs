using CoreDomain;
using CoreDomain.Services;
using CoreDomain.Services.GameStates;
using UnityEngine;

namespace Popups.WinPopup
{
    public class WinPopupController : BasePopupController
    {
        #region --- Members ---

        private readonly string WIN_POPUP_ASSET_NAME = "WinPopup";
        private readonly string WIN_POPUP_BUNDLE_PATH = "Popups/WinPopup";
        private readonly WinPopupView _popupView;

        #endregion


        #region --- Construction ---

        public WinPopupController()
        {
            PopupGO = Object.Instantiate(
                _client.AssetBundleLoaderService.LoadAssetFromBundle<GameObject>(WIN_POPUP_BUNDLE_PATH, WIN_POPUP_ASSET_NAME));

            _popupView = PopupGO.GetComponent<WinPopupView>();
            _popupView.Setup(OnBackToStartScreenClicked);
        }

        #endregion


        #region --- Public Methods ---

        public override void ShowPopup()
        {
            _popupView.Show();
        }

        #endregion


        #region --- Event Handler ---

        private void OnBackToStartScreenClicked()
        {
            DestroyPopup();
            _client.StateMachineService.SwitchState(new LobbyGameState());
        }

        #endregion
    }
}