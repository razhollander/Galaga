using System;
using CoreDomain.Scripts.Utils.Command;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDomain.GameDomain.GameStateDomain.MainGameDomain.Modules.MainGameUi
{
    public class MainGameUiView : MonoBehaviour
    {
        [SerializeField] private Button _shootButton;
        [SerializeField] private FixedJoystick _joystick;
        [SerializeField] private Countable _scoreCountable;

        private Action _shootButtonClickedCallback;
        private Action<float> _onJoystickDraggedCallback;

        public void Setup(Action shootButtonClicked, Action<float> onJoystickDragged)
        {
            _shootButtonClickedCallback = shootButtonClicked;
            _onJoystickDraggedCallback = onJoystickDragged;
        }

        public void UpdateScore(int newScore)
        {
            _scoreCountable.SetNumber(newScore);
        }
        
        private void Awake()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _shootButton.onClick.AddListener(OnShootButtonClicked);
            _joystick.DraggedEvent += OnJoystickDrag;
        }

        private void OnJoystickDrag(Vector2 direction)
        {
            _onJoystickDraggedCallback?.Invoke(direction.x);
        }

        private void RemoveListeners()
        {
            _shootButton.onClick.RemoveListener(OnShootButtonClicked);
            _joystick.DraggedEvent -= OnJoystickDrag;
        }

        private void OnShootButtonClicked()
        {
            _shootButtonClickedCallback?.Invoke();
        }
    }
}