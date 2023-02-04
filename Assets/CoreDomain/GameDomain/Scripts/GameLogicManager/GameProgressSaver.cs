using System;
using UnityEngine;

namespace Features.MainGameScreen.GameLogicManager
{
    public class GameProgressSaver : MonoBehaviour
    {
        #region --- Members ---

        private Action _onApplicationQuit;

        #endregion


        #region --- Mono Override ---

        private void OnApplicationQuit()
        {
            _onApplicationQuit.Invoke();
        }

        #endregion


        #region --- Public Methods ---

        public void Setup(Action onApplicationQuit)
        {
            _onApplicationQuit = onApplicationQuit;
        }

        #endregion
    }
}