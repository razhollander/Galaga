using System;
using Client;
using UnityEngine;
using UnityEngine.UI;

namespace Features.StartScreen
{
    public class StartScreenView : MonoBehaviour
    {
        #region --- Inspector ---

        [SerializeField] private Button _continueFromLastSaveButton;

        #endregion


        #region --- Members ---

        private Action _onContinueFromLastSave;
        private Action _onNewGame;

        #endregion


        #region --- Public Methods ---

        public void Setup(IClient client, Action onNewGame, Action onContinueFromLastSave)
        {
            _onNewGame = onNewGame;
            _onContinueFromLastSave = onContinueFromLastSave;

            _continueFromLastSaveButton.interactable = client.GameSaverManager.DoesHaveLastSave;
        }

        #endregion


        #region --- Event Handler ---

        public void OnContinueFromLastSaveClick()
        {
            _onContinueFromLastSave?.Invoke();
        }

        public void OnNewGameClick()
        {
            _onNewGame?.Invoke();
        }

        #endregion
    }
}