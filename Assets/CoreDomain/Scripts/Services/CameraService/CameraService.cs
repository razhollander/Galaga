using System.Collections.Generic;
using Services.Logs.Base;
using UnityEngine;

namespace CoreDomain.Scripts.Services.CameraService
{
    public class CameraService : ICameraServiceSubscription, ICameraService
    {
        private readonly Dictionary<CameraType, Camera> _cameras = new();

        public void SubscribeCamera(CameraType type, Camera camera)
        {
            var cameraSubscribed = _cameras.ContainsKey(type);

            if (cameraSubscribed)
            {
                LogService.LogError($"{type} camera already subscribed");
            }
            else
            {
                _cameras.Add(type, camera);
            }
        }

        public void UnsubscribeCamera(CameraType type)
        {
            var cameraSubscribed = _cameras.ContainsKey(type);

            if (cameraSubscribed)
            {
                _cameras.Remove(type);
            }
            else
            {
                LogService.LogError("Camera not subscribed");
            }
        }

        public Vector3 WorldToScreenPoint(CameraType type, Vector3 worldPoint)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].WorldToScreenPoint(worldPoint);
        }

        public Vector3 WorldToViewPortPoint(CameraType type, Vector3 worldPoint)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].WorldToViewportPoint(worldPoint);
        }

        public Vector3 ScreenToWorldPoint(CameraType type, Vector3 screenPoint)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].ScreenToWorldPoint(screenPoint);
        }

        public Vector3 ScreenToViewPort(CameraType type, Vector3 screenPoint)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return camera.ScreenToViewportPoint(screenPoint);
            }

            LogService.LogError("camera not found");
            return Vector3.zero;
        }

        public bool ScreenPointToRay(CameraType type, LayerMask layerMask, out RaycastHit hit)
        {
            if (_cameras.TryGetValue(type, out var camera))
            {
                return Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask);
            }

            LogService.LogError("camera not found");
            hit = new RaycastHit();
            return false;
        }

        public bool ScreenPointToRay(CameraType type, out RaycastHit hit)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                hit = new RaycastHit();
                return false;
            }

            return Physics.Raycast(_cameras[type].ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
        }

        public bool ScreenCenterToRay(CameraType type, out RaycastHit hit)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                hit = new RaycastHit();
                return false;
            }

            return Physics.Raycast(_cameras[type].ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0)),
                out hit, Mathf.Infinity);
        }

        public void SetCanvasCamera(CameraType type, Canvas canvas)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return;
            }

            canvas.worldCamera = _cameras[type];
        }

        public Vector2 ScreenPointToCanvasInCameraSpacePoint(RectTransform rect, Vector2 screenPoint, CameraType cameraType)
        {
            var wasSuccess =
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, _cameras[cameraType], out var localPoint);

            if (wasSuccess)
            {
                return localPoint;
            }

            return Vector2.zero;
        }

        public float GetAspectRatio(CameraType type)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
            }

            return _cameras[type].aspect;
        } 
        public Vector3 GetCameraPosition(CameraType type)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].transform.position;
        }

        public Vector3 GetCameraForward(CameraType type)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].transform.forward;
        }

        public Vector3 GetCameraRight(CameraType type)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].transform.right;
        }

        public bool IsPointOnScreen(CameraType type, Vector3 point)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return false;
            }

            var screenPoint = _cameras[type].WorldToScreenPoint(point);

            var isOnScreen = screenPoint.x > 0f
                             && screenPoint.x < Screen.width
                             && screenPoint.y > 0f
                             && screenPoint.y < Screen.height
                             && screenPoint.z > 0f;

            return isOnScreen;
        }
    }
}