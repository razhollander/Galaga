using Managers;
using GameStates;
using UnityEngine;

namespace Popups.DefeatPopup
{
    public class DefeatBasePopupController : BasePopupController
    {
        #region --- Members ---

        private readonly string DEFEAT_POPUP_ASSET_NAME = "DefeatPopup";
        private readonly string DEFEAT_POPUP_BUNDLE_PATH = "Popups/DefeatPopup";
        private readonly DefeatPopupView _popupView;

        #endregion


        #region --- Construction ---

        public DefeatBasePopupController()
        {
            PopupGO = Object.Instantiate(
                _client.AssetBundleSystem.LoadAssetFromBundle<GameObject>(DEFEAT_POPUP_BUNDLE_PATH,
                    DEFEAT_POPUP_ASSET_NAME));

            _popupView = PopupGO.GetComponent<DefeatPopupView>();
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
            _client.StateMachine.SwitchState(new StartScreenState());
        }

        #endregion
    }
}