using UnityEngine;

namespace Handlers
{
    public class CameraManager
    {
        #region --- Members ---

        private Camera _mainCamera;
        public Vector2 ScreenBounds;

        #endregion


        #region --- Construction ---

        public CameraManager()
        {
            _mainCamera = Camera.main;

            ScreenBounds =
                _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
        }

        #endregion


        #region --- Public Methods ---

        public bool IsInScreenHorizontalBounds(float xValue, float spaceKeptFromBounds)
        {
            return xValue > -ScreenBounds.x + spaceKeptFromBounds && xValue < ScreenBounds.x - spaceKeptFromBounds;
        }

        public bool IsInScreenHorizontalBounds(float xValue)
        {
            return xValue > -ScreenBounds.x && xValue < ScreenBounds.x;
        }

        public bool IsInScreenVerticalBounds(float yValue, float spaceKeptFromBounds)
        {
            return yValue > -ScreenBounds.y + spaceKeptFromBounds && yValue < ScreenBounds.y - spaceKeptFromBounds;
        }

        public bool IsInScreenVerticalBounds(float yValue)
        {
            return yValue > -ScreenBounds.y && yValue < ScreenBounds.y;
        }

        #endregion
    }
}