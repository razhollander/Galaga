using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDomain.GameDomain.GameStateDomain.LobbyDomain.Modules.LobbyUi
{
    public class LobbyUiView : MonoBehaviour
    {
        [SerializeField] private Button _quickGameButton;
        [SerializeField] private TMP_InputField _playerNameInputField;
        [SerializeField] private TMP_Dropdown _levelsDropdown;

        private Action _quickGameButtonClickedCallback;
        public string PlayerNameText => _playerNameInputField.text;
        public int SelectedLevel => int.Parse(_levelsDropdown.options[_levelsDropdown.value].text);

        private void Awake()
        {
            AddListeners();
        }

        public void Setup(Action quickGameButtonClickedCallback, int levelsAmount)
        {
            _quickGameButtonClickedCallback = quickGameButtonClickedCallback;
            SetLevelsDropdown(levelsAmount);
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

        private void OnQuickGameButtonClicked()
        {
            _quickGameButtonClickedCallback?.Invoke();
        }

        private void SetLevelsDropdown(int levelsAmount)
        {
            var numberList = Enumerable.Range(1, levelsAmount).Select(x => x.ToString()).ToList();
            _levelsDropdown.AddOptions(numberList);
        }
    }
}