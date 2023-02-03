using UnityEngine;

namespace CoreDomain.Scripts.Services.CameraService
{
    public interface ICameraServiceSubscription
    {
        void SubscribeCamera(CameraType type, Camera camera);
        void UnsubscribeCamera(CameraType type);
    }
}