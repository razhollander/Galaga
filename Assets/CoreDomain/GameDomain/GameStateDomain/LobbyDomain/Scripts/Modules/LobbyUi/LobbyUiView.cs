using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public class LobbyUiView : MonoBehaviour
    {
        [SerializeField] private Button _quickGameButton;
        [SerializeField] private TMP_InputField _playerNameInputField;

        private Action _quickGameButtonClickedCallback;
        public string PlayerNameText => _playerNameInputField.text;

        public void SetCallbacks(Action quickGameButtonClickedCallback)
        {
            _quickGameButtonClickedCallback = quickGameButtonClickedCallback;

            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _quickGameButton.onClick.AddListener(OnQuickGameButtonClicked);
        }
        
        private void RemoveListeners()
        {
            _quickGameButton.onClick.RemoveListener(OnQuickGameButtonClicked);
        }

        public void OnQuickGameButtonClicked()
        {
            _quickGameButtonClickedCallback?.Invoke();
        }
    }
}