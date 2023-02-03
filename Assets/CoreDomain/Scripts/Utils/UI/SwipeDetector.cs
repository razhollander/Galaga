using System;
using UnityEngine;

namespace CoreDomain.Scripts.Utils.UI
{
    public class SwipeDetector : MonoBehaviour
    {
        public static event Action<SwipeData> SwipeEvent = delegate { };

        [SerializeField]
        private bool _detectSwipeOnlyAfterRelease = false;

        [SerializeField]
        private float _minDistanceForSwipeDirectionCheck = 1f;

        [SerializeField]
        private float _minDistanceForSwipe = 20f;

        private Vector2 _fingerDownPosition;
        private Vector2 _fingerUpPosition;

        public bool IsSwiping { get; private set; }

        private void Update()
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _fingerUpPosition = touch.position;
                    _fingerDownPosition = touch.position;
                }

                if (!_detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {
                    _fingerDownPosition = touch.position;
                    DetectSwipe();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    _fingerDownPosition = touch.position;
                    IsSwiping = false;
                }
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                var verticalDirection = VerticalSwipeDirection.None;
                var horizontalDirection = HorizontalSwipeDirection.None;

                if (IsVerticalSwipe())
                {
                    verticalDirection = _fingerDownPosition.y - _fingerUpPosition.y > 0 ? VerticalSwipeDirection.Up : VerticalSwipeDirection.Down;
                }

                if (IsHorizontalSwipe())
                {
                    horizontalDirection = _fingerDownPosition.x - _fingerUpPosition.x > 0 ? HorizontalSwipeDirection.Right : HorizontalSwipeDirection.Left;
                }

                SendSwipe(verticalDirection, horizontalDirection);

                _fingerUpPosition = _fingerDownPosition;

                IsSwiping = true;
            }
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > _minDistanceForSwipeDirectionCheck;
        }

        private bool IsHorizontalSwipe()
        {
            return HorizontalMovementDistance() > _minDistanceForSwipeDirectionCheck;
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > _minDistanceForSwipe || HorizontalMovementDistance() > _minDistanceForSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.x - _fingerUpPosition.x);
        }

        private void SendSwipe(VerticalSwipeDirection verticalSwipeDirection, HorizontalSwipeDirection horizontalSwipeDirection)
        {
            var swipeData = new SwipeData()
            {
                VerticalDirection = verticalSwipeDirection,
                HorizontalDirection = horizontalSwipeDirection,
                StartPosition = _fingerDownPosition,
                EndPosition = _fingerUpPosition
            };

            SwipeEvent?.Invoke(swipeData);
        }
    }

    public struct SwipeData
    {
        public HorizontalSwipeDirection HorizontalDirection;
        public Vector2 EndPosition;
        public Vector2 StartPosition;
        public VerticalSwipeDirection VerticalDirection;
    }

    public enum VerticalSwipeDirection
    {
        Up,
        Down,
        None
    }

    public enum HorizontalSwipeDirection
    {
        Left,
        Right,
        None
    }
}