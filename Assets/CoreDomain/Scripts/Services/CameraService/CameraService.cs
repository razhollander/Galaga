using System.Collections.Generic;
using UnityEngine;

namespace CoreDomain.Services
{
    public class CameraService : ICameraServiceSubscription, ICameraService
    {
        private readonly Dictionary<GameCameraType, Camera> _cameras = new();

        public void SubscribeCamera(GameCameraType type, Camera camera)
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

        public void UnsubscribeCamera(GameCameraType type)
        {
            var cameraSubscribed = _cameras.ContainsKey(type);

            if (cameraSubscribed)
            {
                _cameras.Remove(type);
            }
            else
            {
                LogService.LogError("camera not subscribed");
            }
        }

        public Vector3 ScreenToWorldPoint(GameCameraType type, Vector3 screenPoint)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].ScreenToWorldPoint(screenPoint);
        }

        public Vector3 GetCameraPosition(GameCameraType type)
        {
            if (!_cameras.ContainsKey(type))
            {
                LogService.LogError("camera not found");
                return Vector3.zero;
            }

            return _cameras[type].transform.position;
        }
    }
}