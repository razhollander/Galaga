using System;
using System.Collections.Generic;
using Client;
using GameStates;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managers
{
    public class PopupsManager
    {
        #region --- Members ---

        private BasePopupController _currentBasePopupShown;
        private bool _isCurrentlyShowingPopup;
        private readonly IClient _client;
        private readonly Queue<BasePopupController> _popupsQueue;
        private readonly RectTransform _popupsParent;

        #endregion


        #region --- Construction ---

        public PopupsManager(IClient client)
        {
            _client = client;
            _popupsQueue = new Queue<BasePopupController>();

            AddListeners();

            var popupsParentGO = new GameObject("PopupsParent", typeof(RectTransform));
            _popupsParent = popupsParentGO.GetComponent<RectTransform>();
        }

        ~PopupsManager()
        {
            RemoveListeners();
        }

        #endregion


        #region --- Public Methods ---

        public void QueuePopup(BasePopupController basePopup)
        {
            basePopup.OnDestroyAction = TryShowNextPopup;
            _popupsQueue.Enqueue(basePopup);

            if (_isCurrentlyShowingPopup)
            {
                return;
            }

            _isCurrentlyShowingPopup = true;
            ShowPopup(_popupsQueue.Dequeue());
        }

        #endregion


        #region --- Private Methods ---

        private void AddListeners()
        {
            _client.Broadcaster.Add<IGameState>(OnGameStateChanged);
        }

        private void RemoveListeners()
        {
            _client.Broadcaster.Remove<IGameState>(OnGameStateChanged);
        }

        private void ShowPopup(BasePopupController basePopup)
        {
            _currentBasePopupShown = basePopup;
            var popupTransform = basePopup.PopupGO.transform;
            popupTransform.SetParent(_popupsParent, true);
            popupTransform.localScale = ConstsHandler.VECTOR3_ONE;
            basePopup.OnDestroyAction = TryShowNextPopup;
            basePopup.ShowPopup();
        }

        private void TryShowNextPopup()
        {
            if (_popupsQueue == null || _popupsQueue.Count <= 0 || _popupsQueue.Peek() == null)
            {
                return;
            }

            var popup = _popupsQueue.Dequeue();

            if (popup != null)
            {
                ShowPopup(popup);
            }
            else
            {
                _currentBasePopupShown = null;
                _isCurrentlyShowingPopup = false;
            }
        }

        #endregion


        #region --- Event Handler ---

        private void OnGameStateChanged(IGameState state)
        {
            _popupsQueue.Clear();

            if (_isCurrentlyShowingPopup)
            {
                _currentBasePopupShown?.DestroyPopup();
            }

            _isCurrentlyShowingPopup = false;
        }

        #endregion
    }

    public abstract class BasePopupController
    {
        #region --- Members ---

        public Action OnDestroyAction;

        public GameObject PopupGO;
        protected IClient _client;

        #endregion


        #region --- Construction ---

        public BasePopupController()
        {
            _client = Client.Client.Instance;
        }

        #endregion


        #region --- Public Methods ---

        public void DestroyPopup()
        {
            Object.Destroy(PopupGO);
            OnDestroyAction?.Invoke();
        }

        public abstract void ShowPopup();

        #endregion
    }
}