using System;
using UnityEngine;

namespace Popups.DefeatPopup
{
    public class DefeatPopupView : MonoBehaviour
    {
        #region --- Members ---

        private Action _onBackToStartScreenClicked;

        #endregion


        #region --- Public Methods ---

        public void Setup(Action onBackToStartScreenClicked)
        {
            _onBackToStartScreenClicked = onBackToStartScreenClicked;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        #endregion


        #region --- Event Handler ---

        public void OnBackToStartScreenClicked()
        {
            _onBackToStartScreenClicked?.Invoke();
        }

        #endregion
    }
}